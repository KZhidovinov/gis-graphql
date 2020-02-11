using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile("appsettings.{Environment}.json")
                            .AddEnvironmentVariables()
                            .Build()
                    );

                    webBuilder.UseStartup<Startup>();
                });
    }
}
