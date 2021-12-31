using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderDoesAlreadyExistTests
    {
        [Fact]
        public async void DoesAlreadyExist_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "LOL";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.DoesEventProviderAlreadyExist(name))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.DoesEventProviderAlreadyExist(name);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void DoesAlreadyExist_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string name = "LOL";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.DoesEventProviderAlreadyExist(name))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.DoesEventProviderAlreadyExist(name);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }

        private OutputResponse<bool> FakeSuccessOutput => new OutputResponse<bool>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = true,
        };
        private OutputResponse<bool> FakeNullStringFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
    }
}
