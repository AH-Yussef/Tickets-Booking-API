using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.CustomerAPITests
{
    public class CustomerRegisterTests
    {
        [Fact]
        public async void Register_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var command = new RegisterCustomerCommand
            {
                Name = "mostafa",
                Password = "facad",
                Email = "hamada@yahoo.com",
            };

            mock.Mock<ICustomerService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Register(command);

            Assert.IsType<CreatedResult>(actualResponse as CreatedResult);
        }
        [Fact]
        public async void Create_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            var command = new RegisterCustomerCommand
            {
                Email = "hamada@yahoo.com",
            };

            mock.Mock<ICustomerService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeInvalidInputFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Register(command);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void Delete_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            var command = new RegisterCustomerCommand
            {
                Name = "mostafa",
                Password = "facad",
                Email = "hamada@yahoo.com",
            };

            mock.Mock<ICustomerService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Register(command);
            Assert.IsType<BadRequestObjectResult>(actualResponse as BadRequestObjectResult);
        }

        private OutputResponse<bool> FakeSuccessOutput => new OutputResponse<bool>
        {
            Success = true,
            StatusCode = HttpStatusCode.Created,
            Message = ResponseMessages.Success,
            Model = true,
        };
        private OutputResponse<bool> FakeInvalidInputFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
        private OutputResponse<bool> FakeDoesntExistFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.BadRequest,
            Message = ResponseMessages.Failure,
        };
    }
}
