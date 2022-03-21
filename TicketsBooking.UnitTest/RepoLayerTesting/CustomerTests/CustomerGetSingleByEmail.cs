using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace TicketsBooking.UnitTest.RepoLayerTesting.CustomerTests
{
    public class CustomerGetSingleByEmail
    {
        [Fact]
        public async void GetSingleByEmailExists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_7")
            .Options;

            using var context = new AppDbContext(options);
            var fakeCustomer = GetFakeData();
            foreach (var fakeData in fakeCustomer)
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var expectedCustomer = fakeCustomer[0];
            var target = "Ali@Test.com";
            //Act
            var customerRepo = new CustomerRepo(context);
            var actual = await customerRepo.GetSingleByEmail(target);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCustomer.Name, actual.Name);
            Assert.Equal(expectedCustomer.Password, actual.Password);
            Assert.Equal(expectedCustomer.Email, actual.Email);
        }
        [Fact]
        public async void GetSingleByEmailDoesntExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_8")
            .Options;

            using var context = new AppDbContext(options);
            var fakeCustomer = GetFakeData();
            foreach (var fakeData in fakeCustomer)
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var expectedCustomer = fakeCustomer[0];
            var target = "Hamada@Test.com";
            //Act
            var customerRepo = new CustomerRepo(context);
            var actual = await customerRepo.GetSingleByEmail(target);
            //Assert
            Assert.Null(actual);
        }

        private List<Customer> GetFakeData()
        {
            var customer_1 = new Customer
            {
                Name = "Ali".ToLower(),
                Email = "ali@test.com".ToLower(),
                Password = BC.HashPassword("12345678aH"),
            };

            var customer_2 = new Customer
            {
                Name = "Mostafa".ToLower(),
                Email = "mostafa@test.com".ToLower(),
                Password = BC.HashPassword("12345679aH"),
            };

            var fakeAdmins = new List<Customer>();
            fakeAdmins.Add(customer_1);
            fakeAdmins.Add(customer_2);
            return fakeAdmins;
        }
    }
}
