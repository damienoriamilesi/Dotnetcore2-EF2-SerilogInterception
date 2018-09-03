using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BethaniePieShop.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace BethaniePieShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = LogHelper.Instance;

            // new LoggerConfiguration()
            //     .Enrich.FromLogContext()
            //     .MinimumLevel.Debug()
            //     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //     .WriteTo.Seq("http://localhost:5341")
            //     .CreateLogger();

            // try
            // {
                // Log.Information("Getting the motors running...");

                var host = BuildWebHost(args);

                if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    using(var scope = host.Services.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        try
                        {
                            var seeder = scope.ServiceProvider.GetService<DbSeeder>();
                            seeder.Seed();
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                host.Run();

            // }
            // catch (Exception ex)
            // {
            //     Log.Fatal(ex, $"Host terminated unexpectedly : {ex.Message}");
            // }
            // finally
            // {
            //     Log.CloseAndFlush();
            // }        
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetUpConfiguration)
                // .UseConfiguration(Configuration)
                // .UseUrls("http://localhost:21999")
                .UseStartup<Startup>()
                //.UseSerilog()
                .Build();

        private static void SetUpConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder
                .AddJsonFile("appsettings.Development.json", false, true)
                .AddJsonFile("appsettings.json", false, true)
                //.AddJsonFile($"appsettings.{GetEnvName() ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static string GetEnvName()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return env;
        }
    }

    public class LogHelper
    {
        public static Logger Instance { get; }

        static LogHelper()
        {
            Instance = new LoggerConfiguration().WriteTo.Seq("http://localhost:5341").CreateLogger();
        }
    }
}
