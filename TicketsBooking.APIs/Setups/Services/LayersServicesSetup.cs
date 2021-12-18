using TicketsBooking.APIs.Setups.Factory;
using TicketsBooking.Application;
using TicketsBooking.Infrastructure;
using TicketsBooking.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.APIs.Setups.Services
{
    public class LayersServicesSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationLayer();
            services.AddInfrastructureLayer(configuration);
            services.AddIntegrationLayer(configuration);
        }
    }
}
