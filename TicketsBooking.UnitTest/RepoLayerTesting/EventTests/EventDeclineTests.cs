using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;


namespace TicketsBooking.UnitTest.RepoLayerTesting.EventTests
{
    public class EventDeclineTests
    {
        [Fact]
        public async void UpdateVerified_Exists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_101")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Events.Add(fakeData);
            }
            context.SaveChanges();
            var expectedEvent = GetFakeData()[0];
            //Act

            var eventRepo = new EventRepo(context);
            await eventRepo.Delete(expectedEvent.EventID);
            var actual = eventRepo.GetSingle(expectedEvent.EventID);
            //Assert
            Assert.NotNull(actual);
            
        }
        private List<Event> GetFakeData()
        {
            var eventRecord_1 = new Event
            {
                EventID = "testid1",
                Provider = null,
                Title = "testTitle",
                Description = "description",
                Location = "location",
                dateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = null,
                Tags = null
            };

            var eventRecord_2 = new Event
            {
                EventID = "testid2",
                Provider = null,
                Title = "testTitle",
                Description = "description",
                Location = "location",
                dateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = null,
                Tags = null
            };

            var fakeEvents = new List<Event>();
            fakeEvents.Add(eventRecord_1);
            fakeEvents.Add(eventRecord_2);
            return fakeEvents;
        }
    }
}
