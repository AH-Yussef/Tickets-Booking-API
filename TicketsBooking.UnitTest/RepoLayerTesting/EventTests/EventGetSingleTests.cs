using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventTests
{
    public class EventGetSingleTests
    {
        [Fact]
        public async void GetSingleTests()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_102")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Events.Add(fakeData);
            }

            context.SaveChanges();
            var expectedEvent = GetFakeData()[0];
            var target = "testid1";

            // Act
            var eventRepo = new EventRepo(context);
            var actual = await eventRepo.GetSingle(target);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEvent.EventID, actual.EventID);
            Assert.Equal(expectedEvent.Title, actual.Title);
            Assert.Equal(expectedEvent.Description, actual.Description);
            Assert.Equal(expectedEvent.Location, actual.Location);
            Assert.Equal(expectedEvent.AllTickets, actual.AllTickets);
            Assert.Equal(expectedEvent.SingleTicketPrice, actual.SingleTicketPrice);
            Assert.Equal(expectedEvent.Category, actual.Category);
            Assert.Equal(expectedEvent.SubCategory, actual.SubCategory);
            //Assert.Equal(expectedEvent.Participants, actual.Participants);
            //Assert.Equal(expectedEvent.Tags, actual.Tags);
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
