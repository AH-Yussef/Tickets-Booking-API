using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderGetSingleTests
    {
        [Fact]
        public async void GetSingle()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.EventProviders.Add(fakeData);
            }
            context.SaveChanges();

            var expectedEventProvider = GetFakeData()[0];
            //Act
            var eventProviderRepo = new EventProviderRepo(context);
            var actual = await eventProviderRepo.GetSingle(expectedEventProvider.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEventProvider.Name, actual.Name);
            Assert.Equal(expectedEventProvider.Password, actual.Password);
            Assert.Equal(expectedEventProvider.Email, actual.Email);
        }
        private List<EventProvider> GetFakeData()
        {
            var eventProvider_1 = new EventProvider
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
            };

            var eventProvider_2 = new EventProvider
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
            };

            var fakeAdmins = new List<EventProvider>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            return fakeAdmins;
        }
    }
}
