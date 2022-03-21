using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderDeleteTest
    {
        [Fact]
        public async void Delete_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "lol";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Decline(name))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Decline(name);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void Delete_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string name = null;

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Decline(name))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Decline(name);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void Delete_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "hamada";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Decline(name))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Decline(name);
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
