using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases.DTOs.RepoDTO;
using TicketsBooking.Application.Components.SocialMedias.DTOs;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.PurchaseTests
{
    public class PurchaseCreateTests
    {
        [Fact]
        public async void CreateTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "CreateTest")
                            .Options;

            using var context = new AppDbContext(options);

            var purchaseRepo = new PurchaseRepo(context);
            var customerRepo = new CustomerRepo(context);
            var eventRepo = new EventRepo(context);
            var eventProviderRepo = new EventProviderRepo(context);

            List<string> participants = new List<string>();
            participants.Add("ahmed/role1/team1");

            var fakeCustomer = new RegisterCustomerCommand
            {
                Name = "cid",
                Email = "mostafa@test.com",
                Password = "123456789aH",
            };
            await customerRepo.Register(fakeCustomer);
            context.SaveChanges();


            CreateEventProviderCommand fakeEventProvider = new CreateEventProviderCommand
            {
                Name = "lol",
                Email = "ali@test.com",
                Password = "123456789aH",
                Bio = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo",
                WebsiteLink = "www.test_ali.com",
                SocialMedias = new List<SocialMediaEntry>(),
            };
            await eventProviderRepo.Create(fakeEventProvider);
            context.SaveChanges();

            CreateNewEventCommand fakeEvent = new CreateNewEventCommand
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

            await eventRepo.Create(fakeEvent);
            context.SaveChanges();

            CreateNewPurchaseCommand purchaseCommand = new CreateNewPurchaseCommand
            {
                CustomerID = "mostafa@test.com",
                EventID = "loltesttitle1",
                TicketsCount = 3
            };
            await purchaseRepo.CreateNewPurchase(purchaseCommand);
            context.SaveChanges();

            var expectedPurchase = new PurchaseRepoDTO
            {
                CustomerID = "mostafa@test.com",
                EventID = "loltesttitle1",
                TicketsCount = 3
            };
            // Act
            var actual = await purchaseRepo.GetSingle("mostafa@test.comloltesttitle10");
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedPurchase.CustomerID, actual.CustomerID);
            Assert.Equal(expectedPurchase.EventID, actual.EventID);
            Assert.Equal(expectedPurchase.TicketsCount, actual.TicketsCount);

        }

    }
}
