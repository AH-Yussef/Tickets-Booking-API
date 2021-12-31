using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;
namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderDeleteTests
    {
        [Fact]
        public async void Delete()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_2")
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
            await eventProviderRepo.Delete(expectedEventProvider.Name);
            var actual = await eventProviderRepo.GetSingleByName(expectedEventProvider.Name);
            //Assert
            Assert.Null(actual);
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
