using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.CustomerTests
{
    public class CustomerApproveTests
    {
        [Fact]
        public async void Approve_Exits()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_5")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();
            var FakeEmail = "ali@test.com";
            var customerRepo = new CustomerRepo(context);
            //Act
            await customerRepo.Approve(FakeEmail);
            var actual = await customerRepo.GetSingleByEmail(FakeEmail);
            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Accepted);
        }
        private List<Customer> GetFakeData()
        {
            var customer_1 = new Customer
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "12345678aH",
                Accepted = true
            };

            var customer_2 = new Customer
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "12345678aH",
                Accepted = false
            };
            var customer_3 = new Customer
            {
                Name = "Shosh",
                Email = "Shosh@test.com",
                Password = "12345678aH",
                Accepted = false
            };
            var customer_4 = new Customer
            {
                Name = "Tarek",
                Email = "Tarek@test.com",
                Password = "12345678aH",
                Accepted = false
            };
            var customer_5 = new Customer
            {
                Name = "Nasr",
                Email = "Nasr@test.com",
                Password = "12345678aH",
                Accepted = false
            };

            var fakeCustomers = new List<Customer>();
            fakeCustomers.Add(customer_1);
            fakeCustomers.Add(customer_2);
            fakeCustomers.Add(customer_3);
            fakeCustomers.Add(customer_4);
            fakeCustomers.Add(customer_5);
            return fakeCustomers;
        }
    }
}
