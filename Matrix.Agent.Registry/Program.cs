﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Matrix.Agent.Registry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder();
            host.UseKestrel();
            host.UseContentRoot(Directory.GetCurrentDirectory());

            host.ConfigureAppConfiguration((context, configuration) =>
            {
                var environment = context.HostingEnvironment;

                configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configuration.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                configuration.AddJsonFile($"/run/secrets/appsettings_shared.json", optional: true, reloadOnChange: true);
                configuration.AddJsonFile($"/run/secrets/appsettings_registry.json", optional: true, reloadOnChange: true);
                configuration.AddEnvironmentVariables();
            });

            host.ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
            });

            host.UseStartup<Startup>();
            host.Build().Run();
        }
    }
}