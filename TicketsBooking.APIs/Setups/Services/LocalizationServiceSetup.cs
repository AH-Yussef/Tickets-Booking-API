using System.Collections.Generic;
using System.Globalization;
using TicketsBooking.APIs.Setups.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketsBooking.APIs.Setups.Services
{
    public class LocalizationServiceSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCulters = new List<CultureInfo> {
                    new CultureInfo("ar-SA"),
                    new CultureInfo("en-US")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCulters;
                options.SupportedUICultures = supportedCulters;
            });
        }
    }
}
