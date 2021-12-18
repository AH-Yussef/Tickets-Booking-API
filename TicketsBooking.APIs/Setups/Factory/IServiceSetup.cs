using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.APIs.Setups.Factory
{
    public interface IServiceSetup
    {
        void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}
