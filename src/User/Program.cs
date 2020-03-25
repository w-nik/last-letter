using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace User
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .Build();

            IWebHost myHost = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseUrls(config.GetSection("BaseUrl").Value)
                .UseStartup<Startup>()
                .Build();

            myHost.Run();
        }
    }
}
