using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerApproveTests
    {
        [Fact]
        public async void Approve_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string fakeEmail = null;
            var fakeCommand = new AcceptCustomerCommand
            {
                Email = fakeEmail,
                Token = "123456789",
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Approve(fakeEmail))
                .Returns(Task.FromResult(false));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail));

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await customerService.Approve(fakeCommand);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Approve(fakeEmail), Times.Never);

            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void Approve_ValidDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEmail = "LOL";
            var fakeCommand = new AcceptCustomerCommand
            {
                Email = fakeEmail,
                Token = "123456789",
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Approve(fakeEmail))
                .Returns(Task.FromResult(false));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult((Customer)null));

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = false,
            };
            //Act
            var actualResponse = await customerService.Approve(fakeCommand);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Approve(fakeEmail), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void Approve_ValidExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEmail = "LOL";
            var fakeCommand = new AcceptCustomerCommand
            {
                Email = fakeEmail,
                Token = "123456789",
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Approve(fakeEmail))
                .Returns(Task.FromResult(true));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(new Customer
                {
                    Name = "LOL",
                    Email = "LOL@test.com",
                    ValidationToken = "123456789"
                }));; ;

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
            //Act
            var actualResponse = await customerService.Approve(fakeCommand);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Approve(fakeEmail), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        // GetAll tests

        // GetSingle tests


        private List<Customer> GetSampleCustomers()
        {
            var sample = new List<Customer>();
            sample.Add(new Customer
            {
                Name = "LOL",
            });
            sample.Add(new Customer
            {
                Name = "Mostafa",
            });
            sample.Add(new Customer
            {
                Name = "Tarek",
            });
            sample.Add(new Customer
            {
                Name = "Shosh",
            });

            return sample;
        }
    }
}
