using TicketsBooking.APIs.Setups;
using TicketsBooking.APIs.Setups.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketsBooking.APIs.Setups.Factory;
using TicketsBooking.Application.Common.Responses;
using Microsoft.AspNetCore.Hosting;
using TicketsBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TicketsBooking.APIs
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration.BuildConfigurationFiles(env);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.SetupSwagger();
            app.UseWebSockets();
            app.SetupLocalizationBuilder();
            app.SetupServingContextBuilder();
            app.SetupApplicationAuthenticationAndAuthorization();
            app.SetupCorsBuilder();
            app.UseFluentValidationExceptionHandler();
            app.SetupEndpointBuilder();
        }
    }
}
