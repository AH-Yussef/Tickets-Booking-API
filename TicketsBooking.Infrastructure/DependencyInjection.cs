using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //Setup Database
            services
                .AddDbContext<AppDbContext>(options =>
                {
                    options.UseMySQL(configuration.GetConnectionString("mySql"), builder =>
                    {
                        builder.MigrationsHistoryTable("Migrations");
                    });
                });

            return services;
        }
    }
}
