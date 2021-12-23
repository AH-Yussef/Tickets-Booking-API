using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;
namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderGetAllTests
    {
        [Fact]
        public async void GetAll()
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

            var providerDTO = new GetAllEventProvidersQuery
            {
                pageNumber = 1,

                pageSize = 2,
                
                isVerified = false
             };
            var user1 = GetFakeData()[1];
            var user2 = GetFakeData()[2];
            //Act
            var eventProviderRepo = new EventProviderRepo(context);

            var actual = await eventProviderRepo.GetAll(providerDTO);

            //var actual = await eventProviderRepo.GetSingle(user1.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(actual[0].Name, user1.Name);
            Assert.Equal(actual[0].Email, user1.Email);
            Assert.Equal(actual[0].Password, user1.Password);
            Assert.Equal(actual[0].Verified, user1.Verified);

            Assert.Equal(actual[1].Name, user2.Name);
            Assert.Equal(actual[1].Email, user2.Email);
            Assert.Equal(actual[1].Password, user2.Password);
            Assert.Equal(actual[1].Verified, user2.Verified);
        }
        private List<EventProvider> GetFakeData()
        {
            var eventProvider_1 = new EventProvider
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
                Verified = true
            };

            var eventProvider_2 = new EventProvider
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
                Verified = false
            };
            var eventProvider_3 = new EventProvider
            {
                Name = "Shosh",
                Email = "Shosh@test.com",
                Password = "12345678aH",
                Verified = false
            };

            var fakeAdmins = new List<EventProvider>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            fakeAdmins.Add(eventProvider_3);
            return fakeAdmins;
        }
    }
}
