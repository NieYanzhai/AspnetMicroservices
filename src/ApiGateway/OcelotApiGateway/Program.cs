using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OcelotApiGateway
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
                    webBuilder
                        // .UseKestrel()
                        // .UseUrls("http://0.0.0.0:80")
                        .ConfigureAppConfiguration((hostContext, config) => 
                            config.AddJsonFile($"ocelot.{hostContext.HostingEnvironment.EnvironmentName.ToLower()}.json"))
                        .UseStartup<Startup>();
                });
    }
}
