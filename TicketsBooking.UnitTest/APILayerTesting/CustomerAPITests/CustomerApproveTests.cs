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
    public class CustomerApproveTests
    {
        [Fact]
        public async void Approve_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var fakeEmail = "hamada@yahoo.com";

            var fakeCommand = new AcceptCustomerCommand
            {
                Email = fakeEmail,
                Token = "123456789",
            };

            mock.Mock<ICustomerService>()
                .Setup(service => service.Approve(fakeCommand))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Approve(fakeCommand);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }

        [Fact]
        public async void Approve_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            var fakeEmail = "hamada@yahoo.com";
            var fakeCommand = new AcceptCustomerCommand
            {
                Email = fakeEmail,
                Token = "123456789",
            };

            mock.Mock<ICustomerService>()
                .Setup(service => service.Approve(fakeCommand))
                .Returns(Task.FromResult(FakeInvalidInputFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Approve(fakeCommand);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }

        private OutputResponse<bool> FakeSuccessOutput => new OutputResponse<bool>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = true,
        };
        private OutputResponse<bool> FakeInvalidInputFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
    }
}
