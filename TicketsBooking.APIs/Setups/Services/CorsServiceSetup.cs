using TicketsBooking.APIs.Setups.Factory;
using TicketsBooking.APIs.Setups.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.APIs.Setups.Services
{
    public class CorsServiceSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var accessSettings = new AccessSettings();
            configuration.Bind(nameof(accessSettings),accessSettings);

            services.AddCors(options =>
            {
                options.AddPolicy("platform", builder =>
                {
                    // builder.WithOrigins(accessSettings.Origins)
                    //     .SetIsOriginAllowedToAllowWildcardSubdomains()
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
