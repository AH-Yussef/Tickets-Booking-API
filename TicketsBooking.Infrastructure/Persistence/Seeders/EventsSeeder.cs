using System;
using Bogus;
using TicketsBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TicketsBooking.Infrastructure.Persistence.Seeders
{
    public partial class EventSeeder
    {
        protected Faker _faker;
        protected AppDbContext _dbContext;

        public EventSeeder(AppDbContext dbContext, Faker faker)
        {
            _faker = faker;
            _dbContext = dbContext;
        }

        public void Seed()
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Posts");
            
            for (int i = 1; i < 111; i++)
            {
                _dbContext.Events.Add(new Event
                {
                    Id = i.ToString(),
                    Title = _faker.Lorem.Sentence(_faker.Random.Int(3, 7)),
                    Details = _faker.Lorem.Paragraphs(_faker.Random.Int(2, 5)),
                    Place = _faker.Random.Bool() ? _faker.Address.City() : null,
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
