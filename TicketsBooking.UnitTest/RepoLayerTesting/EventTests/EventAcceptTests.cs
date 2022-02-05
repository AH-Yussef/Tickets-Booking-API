using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventTests
{
    public class EventAcceptTests
    {
        [Fact]
        public async void UpdateVerified_Exists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_100")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Events.Add(fakeData);
            }
            context.SaveChanges();
            var verifiedDTO = new SetAcceptedCommand
            {
                ID = "testid1",
                Accepted = true
            };
            var eventRepo = new EventRepo(context);
            var target = "testid1";
            //Act
            await eventRepo.UpdateAccepted(verifiedDTO);
            var actual = await eventRepo.GetSingle(target);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Accepted, verifiedDTO.Accepted);
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
