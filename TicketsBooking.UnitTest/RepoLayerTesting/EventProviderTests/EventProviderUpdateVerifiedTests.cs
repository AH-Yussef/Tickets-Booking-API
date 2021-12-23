using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderUpdateVerifiedTests
    {
        [Fact]
        public async void UpdateVerified_Exits()
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
            var verifiedDTO = new SetVerifiedCommand
            {
                Name = "Mostafa",
                Verified = true
            };
            var expectedEventProvider = GetFakeData()[0];
            //Act
            var eventProviderRepo = new EventProviderRepo(context);
            await eventProviderRepo.UpdateVerified(verifiedDTO);
            var actual = await eventProviderRepo.GetSingle(expectedEventProvider.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Verified,verifiedDTO.Verified);
        }
        [Fact]
        public async void UpdateVerified_DoesntExits()
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
            var verifiedDTO = new SetVerifiedCommand
            {
                Name = "Hamada",
                Verified = true
            };
            var expectedResponse = false;
            //Act
            var eventProviderRepo = new EventProviderRepo(context);
            var actual = await eventProviderRepo.UpdateVerified(verifiedDTO);
            //Assert
            Assert.Equal(expectedResponse, actual);
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

            var fakeAdmins = new List<EventProvider>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            return fakeAdmins;
        }
    }
}
