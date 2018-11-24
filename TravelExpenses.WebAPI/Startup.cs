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
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAzureWebAppDiagnostics(
                new AzureAppServicesDiagnosticsSettings
                {
                    OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
                });

            logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation("In the ctor");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            int i = 0;
            logger.LogInformation("In ConfigureServices");
            services.AddTransient<IDateTime, MachineDateTime>();
            logger.LogInformation((++i).ToString());
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            logger.LogInformation((++i).ToString());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            logger.LogInformation((++i).ToString());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            logger.LogInformation((++i).ToString());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            logger.LogInformation((++i).ToString());
            services.AddMediatR(typeof(GetAuthenticatedUser.Handler).GetTypeInfo().Assembly);
            logger.LogInformation((++i).ToString());

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUser.Validator>());
            logger.LogInformation((++i).ToString());

            var connectionString = Configuration.GetConnectionString("MyDbConnection");
            logger.LogInformation($"connectionString: {connectionString}");
            logger.LogInformation((++i).ToString());

            services.AddDbContext<TravelExpensesContext>
                (options => options.UseSqlServer(connectionString));
            logger.LogInformation((++i).ToString());

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<TravelExpensesContext>().Database.Migrate();
            logger.LogInformation((++i).ToString());

            services.AddHealthChecks()
                .AddCheck("Database", new SqlConnectionHealthCheck(connectionString))
                .AddGCInfoCheck("GCInfo");

            logger.LogInformation((++i).ToString());

            services.AddAutoMapper();
            logger.LogInformation((++i).ToString());

            //configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            logger.LogInformation((++i).ToString());

            //configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            logger.LogInformation((++i).ToString());
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            logger.LogInformation($"appSettings.Secret: {appSettings.Secret}");
            logger.LogInformation((++i).ToString());
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
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            logger.LogInformation((++i).ToString());

            // We are having FluentValidation do the validations
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            logger.LogInformation("made it to the end");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            logger.LogInformation("Configure");
            app.UseDefaultFiles();
            logger.LogInformation("Configure2");
            app.UseStaticFiles();
            logger.LogInformation("Configure3");

            if (env.IsDevelopment())
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.With(new ThreadIdEnricher())
                    .ReadFrom
                    .Configuration(Configuration)
                    .CreateLogger();

                //app.UseDeveloperExceptionPage();
                loggerFactory.AddSerilog();
                logger.LogInformation("Configure4");
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                logger.LogInformation("Configure5");
                app.UseHsts();
                logger.LogInformation("Configure6");

                loggerFactory.AddAzureWebAppDiagnostics(
                    new AzureAppServicesDiagnosticsSettings
                    {
                        OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
                    });
                logger.LogInformation("Configure7");
            }

            var logger2 = loggerFactory.CreateLogger<Startup>();

            logger2.LogInformation("We are inside Startup talking via logger 2");

            app.ConfigureCustomExceptionMiddleware();
            logger2.LogInformation("2");

            app.UseHttpsRedirection();
            logger2.LogInformation("3");

            // This will register the health checks middleware at the URL /health
            // 
            // This example overrides the HealthCheckResponseWriter to write the health
            // check result in a totally custom way.
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                // This custom writer formats the detailed status as JSON.
                ResponseWriter = WriteResponse,
            });
            logger2.LogInformation("4");

            app.UseMvc();
            logger2.LogInformation("5");
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
