using TicketsBooking.APIs.Setups.Factory;
using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Builders
{
    public static class EndpointBuilderSetup
    {
        public static void SetupEndpointBuilder(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
