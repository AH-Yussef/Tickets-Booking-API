using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.SocialMedias.DTOs;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace TicketsBooking.UnitTest.RepoLayerTesting.EventProviderTests
{
    public class EventProviderCreateTests
    {
        [Fact]
        public async void Create()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_1")
            .Options;

            using var context = new AppDbContext(options);
            var eventProviderRepo = new EventProviderRepo(context);
            foreach (var fakeData in GetFakeData())
            {
                await eventProviderRepo.Create(fakeData);
            }
            var expectedEventProvider = GetFakeData()[0];
            //Act

            var actual = await eventProviderRepo.GetSingleByName(expectedEventProvider.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedEventProvider.Name.ToLower(), actual.Name);
            Assert.True(BC.Verify(expectedEventProvider.Password, actual.Password));
            Assert.Equal(expectedEventProvider.Email, actual.Email);
        }
        private List<CreateEventProviderCommand> GetFakeData()
        {
            var eventProvider_1 = new CreateEventProviderCommand
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "123456789aH",
                Bio = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo",
                WebsiteLink = "www.test_ali.com",
                SocialMedias = new List<SocialMediaEntry>(),
            };
            eventProvider_1.SocialMedias.Add(new SocialMediaEntry
            {
                Type = "facebook",
                Link = "www.facbook.com/ali_page",
            });

            var eventProvider_2 = new CreateEventProviderCommand
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "123456789aH",
                Bio = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo",
                WebsiteLink = "www.test_mostafa.com",
                SocialMedias = null,
            };

            var fakeAdmins = new List<CreateEventProviderCommand>();
            fakeAdmins.Add(eventProvider_1);
            fakeAdmins.Add(eventProvider_2);
            return fakeAdmins;
        }
    }
}
