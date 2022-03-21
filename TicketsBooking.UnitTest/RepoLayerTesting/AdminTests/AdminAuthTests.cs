using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.AdminTests
{
    public class AdminAuthTests
    {
        [Fact]
        public async void GetSingle()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach(var fakeAdmin in GetFakeAdmins())
            {
                context.Admins.Add(fakeAdmin);
            }
            context.SaveChanges();

            var expectedAdmin = GetFakeAdmins()[0];
            //Act
            var adminRepo = new AdminRepo(context);
            var actual = await adminRepo.GetSingle(expectedAdmin.Email);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.Name, expectedAdmin.Name);
            Assert.Equal(actual.Email, expectedAdmin.Email);
            Assert.Equal(actual.Password, expectedAdmin.Password);
        }

        private List<Admin> GetFakeAdmins()
        {
            var admin_1 = new Admin
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
            };

            var admin_2 = new Admin
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
            };

            var fakeAdmins = new List<Admin>();
            fakeAdmins.Add(admin_1);
            fakeAdmins.Add(admin_2);
            return fakeAdmins;
        }
    }
}
