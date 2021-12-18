using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.Integration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIntegrationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
