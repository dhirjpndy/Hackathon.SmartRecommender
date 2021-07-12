using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Hackathon.SmartRecommender.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
               // logger.Debug("Starting ConsumerMarketplace.AdminFlexTool");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                //logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
               // NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             new HostBuilder()
                .ConfigureWebHost(whb =>
                {
                    whb.UseShutdownTimeout(TimeSpan.FromSeconds(25));
                    whb.UseKestrel();
                    whb.UseStartup<Startup>();
                })
                .ConfigureLogging((hbc, lb) =>
                {
                    lb.ClearProviders();
                    lb.AddConfiguration(hbc.Configuration.GetSection("Logging"));
                    lb.AddDebug();
                    lb.AddEventSourceLogger();
                })
                .ConfigureAppConfiguration((hbc, cb) =>
                {
                    /*
                     * When run in Kubernetes, these files will be populated from configmaps and secrets
                     * specific to the environment. Each file overrides some portion of the default
                     * appsettings.json, which is what is used when running the project locally.
                     */
                    cb.AddJsonFile("appsettings.json", false, false);
                    cb.AddJsonFile("/k8s/config/appsettings.json", true, false);
                    cb.AddJsonFile("/k8s/secrets/appsettings.json", true, false);
                })
                .UseNLog();


        //Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
