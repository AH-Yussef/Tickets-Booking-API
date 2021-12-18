using System.Text.Json;
using TicketsBooking.APIs.Setups.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.APIs.Setups.Services
{
    public class JsonNamingServiceSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }
    }
}
