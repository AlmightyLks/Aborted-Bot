using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using SynapseBot.Configs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureLogging((ctx, logBuilder) =>
                {
                    Log.Logger = new LoggerConfiguration()
                     .WriteTo
                     .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                     .WriteTo
                     .File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log-.txt"),
                         LogEventLevel.Error,
                         fileSizeLimitBytes: 5242880, //50 MB
                         rollOnFileSizeLimit: true,
                         rollingInterval: RollingInterval.Hour
                         )
                     .CreateLogger();
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddSingleton<SerilogLoggerFactory>();
                    services.AddSingleton(services);

                    services.AddHostedService<Bot>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
