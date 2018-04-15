using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace slogram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseCloudFoundryHosting()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                //.UseUrls("http://0.0.0.0:9000")
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.AddConsole(); //This is for cf
                })
                .Build();

            hostBuilder.Run();
        }
    }
}
