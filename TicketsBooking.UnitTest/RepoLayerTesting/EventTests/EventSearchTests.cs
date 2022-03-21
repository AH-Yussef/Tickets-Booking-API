using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventTests
{
    public class EventSearchTests
    {
        [Fact]
        public async void SearchTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "SearchTest")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Events.Add(fakeData);
            }

            context.SaveChanges();
            /*var gaeq = new GetAllEventsQuery
            {
                PageNumber = 1,
                PageSize = 2,
                Accepted = false
            };*/
            string query = "category1 category2";
            var expectedEvent1 = GetFakeData()[0];
            var expectedEvent2 = GetFakeData()[1];

            // Act
            var eventRepo = new EventRepo(context);
            var actual = await eventRepo.Search(query);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEvent1.EventID, actual[0].EventID);
            Assert.Equal(expectedEvent1.Title, actual[0].Title);
            Assert.Equal(expectedEvent1.Description, actual[0].Description);
            Assert.Equal(expectedEvent1.Location, actual[0].Location);
            Assert.Equal(expectedEvent1.AllTickets, actual[0].AllTickets);
            Assert.Equal(expectedEvent1.SingleTicketPrice, actual[0].SingleTicketPrice);
            Assert.Equal(expectedEvent1.Category, actual[0].Category);
            Assert.Equal(expectedEvent1.SubCategory, actual[0].SubCategory);

            Assert.NotNull(actual);
            Assert.Equal(expectedEvent2.EventID, actual[1].EventID);
            Assert.Equal(expectedEvent2.Title, actual[1].Title);
            Assert.Equal(expectedEvent2.Description, actual[1].Description);
            Assert.Equal(expectedEvent2.Location, actual[1].Location);
            Assert.Equal(expectedEvent2.AllTickets, actual[1].AllTickets);
            Assert.Equal(expectedEvent2.SingleTicketPrice, actual[1].SingleTicketPrice);
            Assert.Equal(expectedEvent2.Category, actual[1].Category);
            Assert.Equal(expectedEvent2.SubCategory, actual[1].SubCategory);
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
                Accepted = true,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now + new TimeSpan(20, 20, 20),
                Category = "category1",
                SubCategory = "category",
                Participants = null,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Keyword = "category1"
                    }
                }
            };

            var eventRecord_2 = new Event
            {
                EventID = "testid2",
                
                Provider = null,
                Title = "testTitle",
                Description = "description",
                Location = "location",
                dateTime = System.DateTime.Now,
                Accepted = true,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now + new TimeSpan(20, 20, 20),
                Category = "category2",
                SubCategory = "category",
                Participants = null,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Keyword = "category2"
                    }
                }
            };

            var fakeEvents = new List<Event>();
            fakeEvents.Add(eventRecord_1);
            fakeEvents.Add(eventRecord_2);
            return fakeEvents;
        }
    }
}
