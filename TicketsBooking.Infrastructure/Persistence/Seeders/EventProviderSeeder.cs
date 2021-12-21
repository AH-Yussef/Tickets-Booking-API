using Bogus;
using TicketsBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TicketsBooking.Infrastructure.Persistence.Seeders
{
    public partial class EventProviderSeeder
    {
        protected Faker _faker;
        protected AppDbContext _dbContext;

        public EventProviderSeeder(AppDbContext dbContext, Faker faker)
        {
            _faker = faker;
            _dbContext = dbContext;
        }

        public void Seed()
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM EventProviders");
            
            for (int i = 1; i < 10; i++)
            {
                _dbContext.EventProviders.Add(new EventProvider
                {
                    Name = _faker.Lorem.Sentence(_faker.Random.Int(3, 7)),
                    Email = _faker.Internet.Email(),
                    Bio = _faker.Lorem.Paragraphs(_faker.Random.Int(2, 5)),
                    Verified = _faker.Random.Bool(),
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
