using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using TravelExpenses.Application.Features;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Application.Infrastructure;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Common;
using TravelExpenses.Infrastructure;
using TravelExpenses.Persistence;
using TravelExpenses.WebAPI.Extensions;
using TravelExpenses.WebAPI.HealthChecks;
using TravelExpenses.WebAPI.Middleware;

namespace TravelExpenses.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetAuthenticatedUser.Handler).GetTypeInfo().Assembly);
            
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUser.Validator>());

            var connectionString = Configuration.GetConnectionString("MyDbConnection");

            services.AddDbContext<TravelExpensesContext>
                (options => options.UseSqlServer(connectionString));

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment != null && environment != "Development")
            {
                // Automatically perform database migration
                // The integration tests cannot run this command
                services.BuildServiceProvider().GetService<TravelExpensesContext>().Database.Migrate();
            }

            services.AddHealthChecks()
                .AddEnvironmentCheck()
                .AddCheck("Database", new SqlConnectionHealthCheck(connectionString))
                .AddGCInfoCheck("GCInfo");
            
            services.AddAutoMapper();

            //configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RequireSignedTokens = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
            
            // We are having FluentValidation do the validations
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // https://www.blinkingcaret.com/2018/01/24/angular-and-asp-net-core/
            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                await next.Invoke();

                // This is for when we are using SPA routing such as /customers/123
                // This is obviously not for the Web API, so we pass it to the client
                if (context.Response.StatusCode == 404 && !context.Request.Path.Value.Contains("/api"))
                {
                    context.Request.Path = new PathString("/index.html");
                    await next.Invoke();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.With(new ThreadIdEnricher())
                    .ReadFrom
                    .Configuration(Configuration)
                    .CreateLogger();
                
                loggerFactory.AddSerilog();

                // As we are calling npm run serve in VS Code, it will be on a different port than the Web API and requires CORS config
                app.UseCors(builder => 
                    builder.WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod());

                //app.UseMiddleware<DevelopmentLatencyMiddleware>();
            }
            else
            {
                app.UseHsts();

                loggerFactory.AddAzureWebAppDiagnostics(
                    new AzureAppServicesDiagnosticsSettings
                    {
                        OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
                    });
            }

            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogDebug("The logger is now available");

            app.ConfigureCustomExceptionMiddleware();            

            app.UseHttpsRedirection();

            // This will register the health checks middleware at the URL /health
            // 
            // This example overrides the HealthCheckResponseWriter to write the health
            // check result in a totally custom way.
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                // This custom writer formats the detailed status as JSON.
                ResponseWriter = WriteResponse,
            });

            app.UseAuthentication();

            app.UseMvc();
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
