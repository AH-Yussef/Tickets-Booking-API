using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Crosscut.Constants;
using Xunit;
namespace TicketsBooking.UnitTest.APILayerTesting.CustomerAPITests
{
    public class CustomerDeleteTests
    {
        [Fact]
        public async void Delete_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string Email = "lol";

            mock.Mock<ICustomerService>()
                .Setup(service => service.Delete(Email))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Delete(Email);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void Delete_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string Email = null;

            mock.Mock<ICustomerService>()
                .Setup(service => service.Delete(Email))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Delete(Email);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void Delete_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            string Email = "hamada";

            mock.Mock<ICustomerService>()
                .Setup(service => service.Delete(Email))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var customerController = mock.Create<CustomerController>();
            var actualResponse = await customerController.Delete(Email);
            Assert.IsType<NotFoundObjectResult>(actualResponse as NotFoundObjectResult);
        }

        private OutputResponse<bool> FakeSuccessOutput => new OutputResponse<bool>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
        };
        private OutputResponse<bool> FakeNullStringFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
        private OutputResponse<bool> FakeDoesntExistFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.NotFound,
            Message = ResponseMessages.Failure,
        };
    }
}
