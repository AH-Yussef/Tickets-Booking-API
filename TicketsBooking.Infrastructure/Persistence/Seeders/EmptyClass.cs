using System;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TicketsBooking.Infrastructure.Persistence.Seeders
{
    public partial class DatabaseSeeder
    {
        protected Faker _faker;
        protected AppDbContext _dbContext;

        public DatabaseSeeder(IHost host)
        {
            _faker = new Faker("en");
            _dbContext = host.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
        }

        public void Seed()
        {
            new EventSeeder(_dbContext, _faker).Seed();
        }
    }
}
