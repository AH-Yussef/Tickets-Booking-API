using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketsBooking.APIs.Setups.Factory;

namespace TicketsBooking.APIs.Setups.Services
{
    public class MailingSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        }
    }
}
