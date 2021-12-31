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

namespace TicketsBooking.UnitTest.APILayerTesting
{
    public class EventProviderControllerTests
    {
        [Fact]
        public async void Authenticate_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var authCreds = new AuthCreds
            {
                Email = "test@test.com",
                Password = "123456789aH",
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Authenticate(authCreds))
                .Returns(Task.FromResult(FakeSuccessOuput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Authenticate(authCreds);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }

        [Fact]
        public async void Authenticate_UnAuthorizedResponse()
        {
            using var mock = AutoMock.GetLoose();

            var authCreds = new AuthCreds
            {
                Email = "test@test.com",
                Password = "123456789aH",
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Authenticate(authCreds))
                .Returns(Task.FromResult(FakeFailureOuput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Authenticate(authCreds);

            Assert.IsType<UnauthorizedObjectResult>(actualResponse as UnauthorizedObjectResult);
        }

        [Fact]
        public async void Authenticate_UnprocessableEntity()
        {
            using var mock = AutoMock.GetLoose();
            var authCreds = new AuthCreds
            {
                Email = null,
                Password = "123456789aH",
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Authenticate(authCreds))
                .Returns(Task.FromResult(FakeInValidResponse));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Authenticate(authCreds);

            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }

        private OutputResponse<AuthedUserResult> FakeSuccessOuput => new OutputResponse<AuthedUserResult>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = new AuthedUserResult {
                Name = "test",
                Email = "test@test.com",
                Token = "123456789",
            },
        };

        private OutputResponse<AuthedUserResult> FakeFailureOuput => new OutputResponse<AuthedUserResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.Unauthorized,
            Message = ResponseMessages.Unauthenticated,
            Model = null
        };

        private OutputResponse<AuthedUserResult> FakeInValidResponse => new OutputResponse<AuthedUserResult>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
            Model = null
        };
    }
}
