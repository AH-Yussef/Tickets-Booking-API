using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderGetSingleTests
    {
        [Fact]
        public async void GetSingleByName()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_4_1")
            .Options;

            using var context = new AppDbContext(options);
            var fakeEventProviders = GetFakeData();
            foreach (var fakeData in fakeEventProviders)
            {
                context.EventProviders.Add(fakeData);
            }
            context.SaveChanges();

            var expectedEventProvider = fakeEventProviders[0];
            var target = "Ali";
            //Act
            var eventProviderRepo = new EventProviderRepo(context);
            var actual = await eventProviderRepo.GetSingleByName(target);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEventProvider.Name, actual.Name);
            Assert.Equal(expectedEventProvider.Password, actual.Password);
            Assert.Equal(expectedEventProvider.Email, actual.Email);
        }

        [Fact]
        public async void GetSingleByEmail()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_4_2")
            .Options;

            using var context = new AppDbContext(options);
            var fakeEventProviders = GetFakeData();
            foreach (var fakeData in fakeEventProviders)
            {
                context.EventProviders.Add(fakeData);
            }
            context.SaveChanges();

            var expectedEventProvider = fakeEventProviders[0];
            var target = "Ali@Test.com";
            //Act
            var eventProviderRepo = new EventProviderRepo(context);
            var actual = await eventProviderRepo.GetSingleByEmail(target);
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
                Name = "Ali".ToLower(),
                Email = "ali@test.com".ToLower(),
                Password = BC.HashPassword("12345678aH"),
            };

            var eventProvider_2 = new EventProvider
            {
                Name = "Mostafa".ToLower(),
                Email = "mostafa@test.com".ToLower(),
                Password = BC.HashPassword("12345679aH"),
            };

            var fakeAdmins = new List<EventProvider>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            return fakeAdmins;
        }
    }
}
