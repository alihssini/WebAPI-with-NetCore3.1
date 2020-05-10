using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;

namespace Ali.Hosseini.Application.Api
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                       .UseKestrel(options =>
                       {
                           options.Listen(IPAddress.Loopback, 5000);
                       })
                       .UseStartup<Startup>();
                });
    }
}
