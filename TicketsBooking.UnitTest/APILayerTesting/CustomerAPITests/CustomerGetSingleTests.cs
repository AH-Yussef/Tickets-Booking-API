using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.CustomerAPITests
{
    public class CustomerGetSingleTests
    {
        [Fact]
        public async void GetSingle_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string Email = "lol";

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(Email))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(Email);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void GetSingle_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string Email = null;

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(Email))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(Email);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void GetSingle_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            string Emal = "hamada";

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(Emal))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(Emal);
            Assert.IsType<NotFoundObjectResult>(actualResponse as NotFoundObjectResult);
        }

        private OutputResponse<GetSingleUserResult> FakeSuccessOutput => new OutputResponse<GetSingleUserResult>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = new GetSingleUserResult()
        };
        private OutputResponse<GetSingleUserResult> FakeNullStringFailOutput => new OutputResponse<GetSingleUserResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
            Model = null,
        };
        private OutputResponse<GetSingleUserResult> FakeDoesntExistFailOutput => new OutputResponse<GetSingleUserResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.NotFound,
            Message = ResponseMessages.Failure,
            Model = null,
        };
    }
}
