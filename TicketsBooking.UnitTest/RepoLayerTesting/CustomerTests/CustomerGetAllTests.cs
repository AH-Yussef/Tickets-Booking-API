using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.RepoLayerTesting.CustomerTests
{
    public class CustomerGetAllTests
    {
        [Fact]
        public async void GetAll_searchTarget()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_2")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var customerDTO = new GetAllUsersQuery
            {
                pageNumber = 1,

                pageSize = 2,

                searchTarget = "Ali"
            };
            var user1 = GetFakeData()[0];
            //Act
            var customerRepo = new CustomerRepo(context);

            var actual = await customerRepo.GetAll(customerDTO);

            //var actual = await eventProviderRepo.GetSingle(user1.Name);
            //Assert
            Assert.NotNull(actual);

            Assert.Equal(actual[0].Name, user1.Name);
            Assert.Equal(actual[0].Email, user1.Email);
            Assert.Equal(actual[0].Password, user1.Password);
            Assert.Equal(actual[0].Accepted, user1.Accepted);
        }
        [Fact]
        public async void GetAll_searchTargetDoesntExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_3")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var customerDTO = new GetAllUsersQuery
            {
                pageNumber = 1,

                pageSize = 2,

                searchTarget = "7amada"
            };
            //Act
            var customerRepo = new CustomerRepo(context);

            var actual = await customerRepo.GetAll(customerDTO);

            //var actual = await eventProviderRepo.GetSingle(user1.Name);
            //Assert
            Assert.NotNull(actual);
        }
        [Fact]
        public async void GetAll_NosearchTarget()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TicketsBookingDb_3_4")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var fakeData in GetFakeData())
            {
                context.Customers.Add(fakeData);
            }
            context.SaveChanges();

            var customerDTO = new GetAllUsersQuery
            {
                pageNumber = 1,

                pageSize = 2,

            };
            var user1 = GetFakeData()[0];
            var user2 = GetFakeData()[1];
            //Act
            var customerRepo = new CustomerRepo(context);

            var actual = await customerRepo.GetAll(customerDTO);

            //var actual = await eventProviderRepo.GetSingle(user1.Name);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);

            Assert.Equal(actual[0].Name, user1.Name);
            Assert.Equal(actual[0].Email, user1.Email);
            Assert.Equal(actual[0].Password, user1.Password);
            Assert.Equal(actual[0].Accepted, user1.Accepted);

            Assert.Equal(actual[1].Name, user2.Name);
            Assert.Equal(actual[1].Email, user2.Email);
            Assert.Equal(actual[1].Password, user2.Password);
            Assert.Equal(actual[1].Accepted, user2.Accepted);
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
