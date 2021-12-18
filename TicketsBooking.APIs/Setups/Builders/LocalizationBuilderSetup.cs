using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;


namespace TicketsBooking.APIs.Setups.Builders
{
    public static class LocalizationBuilderSetup
    {
        public static void SetupLocalizationBuilder(this IApplicationBuilder app)
        {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);
        }
    }
}
