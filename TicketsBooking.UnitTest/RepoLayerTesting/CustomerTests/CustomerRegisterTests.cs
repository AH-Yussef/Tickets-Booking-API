using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

using BC = BCrypt.Net.BCrypt;
namespace TicketsBooking.UnitTest.RepoLayerTesting.CustomerTests
{
    public class CustomerRegisterTests
    {
        [Fact]
        public async void Create()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_1")
            .Options;

            using var context = new AppDbContext(options);
            var customerRepo = new CustomerRepo(context);
            foreach (var fakeData in GetFakeData())
            {
                await customerRepo.Register(fakeData);
            }
            var expectedCustomer = GetFakeData()[0];
            //Act

            var actual = await customerRepo.GetSingleByEmail(expectedCustomer.Email);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCustomer.Name.ToLower(), actual.Name);
            Assert.True(BC.Verify(expectedCustomer.Password, actual.Password));
            Assert.Equal(expectedCustomer.Email, actual.Email);
        }
        private List<RegisterCustomerCommand> GetFakeData()
        {
            var customer_1 = new RegisterCustomerCommand
            {
                Name = "Ali",
                Email = "ali@test.com",
                Password = "123456789aH",
            };

            var customer_2 = new RegisterCustomerCommand
            {
                Name = "Mostafa",
                Email = "mostafa@test.com",
                Password = "123456789aH",
            };

            var fakeCustomers = new List<RegisterCustomerCommand>();
            fakeCustomers.Add(customer_1);
            fakeCustomers.Add(customer_2);
            return fakeCustomers;
        }
    }
}
