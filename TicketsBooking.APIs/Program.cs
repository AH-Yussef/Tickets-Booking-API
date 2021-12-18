using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TicketsBooking.APIs.Setups.Settings;
using TicketsBooking.Infrastructure.Persistence.Seeders;
using Serilog;

namespace TicketsBooking.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // database seeder
            if (args.Length > 0 && args.Any(arg => arg == "seed"))
            {
                new DatabaseSeeder(host).Seed();
                return;
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
