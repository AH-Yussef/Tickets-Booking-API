using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderUpdateVerifiedTests
    {
        [Fact]
        public async void UpdateVerified_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var command = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Approve(command))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.SetVerified(command);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void UpdateVerified_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            var command = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Approve(command))
                .Returns(Task.FromResult(FakeInvalidInputFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.SetVerified(command);
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
