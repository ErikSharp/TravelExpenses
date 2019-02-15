using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Sinks.MSSqlServer;

namespace TravelExpenses.WebAPI
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            SetupLogger();            

            try
            {                
                Log.Information("Starting the server...");

                BuildWebHost(args).Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.Information("Shutting down");
                Log.CloseAndFlush();
            }

            void SetupLogger()
            {
                SelfLog.Enable(msg => Debug.WriteLine(msg));

                var columnOptions = new ColumnOptions();
                columnOptions.Store.Remove(StandardColumn.MessageTemplate);
                columnOptions.Store.Remove(StandardColumn.Properties);

                columnOptions.Level.StoreAsEnum = true;

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    //.Enrich.FromLogContext()
                    .WriteTo.MSSqlServer(
                        connectionString: Configuration.GetConnectionString("MyDbConnection"),
                        tableName: "Logs",
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                        autoCreateSqlTable: true,
                        columnOptions: columnOptions)
                    .WriteTo.AddEnvironmentSpecificLoggers(
                        SetupProductionLogger,
                        SetupDevelopmentLogger)
                    .CreateLogger();

                LoggerConfiguration SetupProductionLogger(LoggerSinkConfiguration config)
                {
                    // https://github.com/serilog/serilog-aspnetcore#writing-to-the-azure-diagnostics-log-stream

                    return config.File(
                        @"D:\home\LogFiles\Application\myapp.txt",
                        fileSizeLimitBytes: 1_000_000,
                        rollOnFileSizeLimit: true,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(3));
                }

                LoggerConfiguration SetupDevelopmentLogger(LoggerSinkConfiguration config)
                {
                    return config.Console(
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
                        outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    );
                }
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseConfiguration(Configuration)
                   //.UseSerilog()
                   .Build();        
    }

    static class ConfigurationExtensions
    {
        public static LoggerConfiguration AddEnvironmentSpecificLoggers(
            this LoggerSinkConfiguration source,
            Func<LoggerSinkConfiguration, LoggerConfiguration> productionConfiguration,
            Func<LoggerSinkConfiguration, LoggerConfiguration> developmentConfiguration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch (env)
            {
                case "Production":
                    return productionConfiguration(source);                    
                case "Development":                    
                default:
                    return developmentConfiguration(source);
            }
        }
    }
}