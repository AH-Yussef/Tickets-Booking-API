using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.CustomerTests
{
    public class CustomerDeleteTests
    {
        [Fact]
        public async void Delete()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_6")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var expectedCustomer = GetFakeData()[0];
            //Act
            var customerRepo = new CustomerRepo(context);
            await customerRepo.Delete(expectedCustomer.Email);
            var actual = await customerRepo.GetSingleByEmail(expectedCustomer.Email);
            //Assert
            Assert.Null(actual);
        }
        private List<Customer> GetFakeData()
        {
            var customer_1 = new Customer
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
            };

            var customer_2 = new Customer
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
            };

            var fakeCustomers = new List<Customer>();
            fakeCustomers.Add(customer_1);
            fakeCustomers.Add(customer_2);
            return fakeCustomers;
        }
    }
}
