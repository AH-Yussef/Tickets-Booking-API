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
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using Xunit;


namespace TicketsBooking.UnitTest.APILayerTesting.CustomerAPITests
{
    public class CustomerGetAllTests
    {
        [Fact]
        public async void GetAll_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "lol";

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(name);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void GetAll_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string name = null;

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(name);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void GetAll_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "hamada";

            mock.Mock<ICustomerService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.GetSingle(name);
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
