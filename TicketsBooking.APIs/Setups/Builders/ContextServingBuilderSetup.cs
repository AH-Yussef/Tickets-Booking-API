using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Builders
{
    public static class ContextServingBuilderSetup 
    {
        public static void SetupServingContextBuilder(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
        }
    }
}
