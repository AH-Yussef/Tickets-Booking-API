using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Builders
{
    public static class CorsBuilderSetup
    {
        public static void SetupCorsBuilder(this IApplicationBuilder app)
        {
            app.UseCors("platform");
        }
    }
}
