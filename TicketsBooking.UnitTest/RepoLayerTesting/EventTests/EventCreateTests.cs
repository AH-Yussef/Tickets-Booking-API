using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventTests
{
    public class EventCreateTests
    {
        [Fact]
        public async void CreateTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_104")
            .Options;

            using var context = new AppDbContext(options);
            
            var eventRepo = new EventRepo(context);
            foreach (var fakeData in GetFakeData())
            {
                await eventRepo.Create(fakeData);
            }
            context.SaveChanges();
            var expectedEvent = GetFakeData()[0];
            

            // Act
            var actual = await eventRepo.GetSingle("loltesttitle1");
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEvent.Title, actual.Title);
            Assert.Equal(expectedEvent.Description, actual.Description);
            Assert.Equal(expectedEvent.Location, actual.Location);
            Assert.Equal(expectedEvent.AllTickets, actual.AllTickets);
            Assert.Equal(expectedEvent.SingleTicketPrice, actual.SingleTicketPrice);
            Assert.Equal(expectedEvent.Category, actual.Category);
            Assert.Equal(expectedEvent.SubCategory, actual.SubCategory);

        }
        private List<CreateNewEventCommand> GetFakeData()
        {
            List<string> participants = new List<string>();
            participants.Add("ahmed/role1/team1");
            

            var eventRecord_1 = new CreateNewEventCommand
            {
                ProviderName = "lol",
                Title = "testtitle1",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "subcategory",
                Participants = participants,
                Tags = new List<string>()
            };

            var fakeEvents = new List<CreateNewEventCommand>();
            fakeEvents.Add(eventRecord_1);
            return fakeEvents;
        }
    }
}
