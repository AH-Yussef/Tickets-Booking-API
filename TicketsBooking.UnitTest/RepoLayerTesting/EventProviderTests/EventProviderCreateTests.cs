using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderCreateTests
    {
        [Fact]
        public async void GetSingle()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb")
            .Options;

            using var context = new AppDbContext(options);
            var eventProviderRepo = new EventProviderRepo(context);
            foreach (var fakeData in GetFakeData())
            {
                await eventProviderRepo.Create(fakeData);
            }

            var expectedEventProvider = GetFakeData()[0];
            //Act
            
            var actual = await eventProviderRepo.GetSingle(expectedEventProvider.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEventProvider.Name, actual.Name);
            Assert.Equal(expectedEventProvider.Password, actual.Password);
            Assert.Equal(expectedEventProvider.Email, actual.Email);
        }
        private List<CreateEventProviderCommand> GetFakeData()
        {
            var eventProvider_1 = new CreateEventProviderCommand
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
            };

            var eventProvider_2 = new CreateEventProviderCommand
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
                Bio = "test",
                
            };

            var fakeAdmins = new List<CreateEventProviderCommand>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            return fakeAdmins;
        }
    }
}
