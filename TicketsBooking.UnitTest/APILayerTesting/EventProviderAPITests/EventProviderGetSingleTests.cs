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
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderGetSingleTests
    {
        [Fact]
        public async void GetAll_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "lol";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.GetSingle(name);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void GetAll_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            string name = null;

            mock.Mock<IEventProviderService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.GetSingle(name);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void GetAll_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            string name = "hamada";

            mock.Mock<IEventProviderService>()
                .Setup(service => service.GetSingle(name))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.GetSingle(name);
            Assert.IsType<NotFoundObjectResult>(actualResponse as NotFoundObjectResult);
        }

        private OutputResponse<EventProviderSingleResult> FakeSuccessOutput => new OutputResponse<EventProviderSingleResult>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = new EventProviderSingleResult()
        };
        private OutputResponse<EventProviderSingleResult> FakeNullStringFailOutput => new OutputResponse<EventProviderSingleResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
            Model = null,
        };
        private OutputResponse<EventProviderSingleResult> FakeDoesntExistFailOutput => new OutputResponse<EventProviderSingleResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.NotFound,
            Message = ResponseMessages.Failure,
            Model = null,
        };
    }
}
