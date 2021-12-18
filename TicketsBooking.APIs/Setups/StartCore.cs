using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TicketsBooking.APIs.Setups
{
    public static class StartCore
    {
        public static IConfiguration BuildConfigurationFiles(this IConfiguration config,IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(env.EnvironmentName))
            {
                var builder = new ConfigurationBuilder();

                if (!string.IsNullOrEmpty(env.ContentRootPath))
                    builder.SetBasePath(env.ContentRootPath);
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                try
                {
                    builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                }
                catch
                {
                    // ignored
                }

                builder.AddEnvironmentVariables();
                config = builder.Build();
                return config;
            }
            else
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                config = builder.Build();
                return config;
            }
        }
    }
}
