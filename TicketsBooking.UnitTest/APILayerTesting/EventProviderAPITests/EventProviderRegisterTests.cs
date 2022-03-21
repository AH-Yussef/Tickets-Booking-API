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
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.SocialMedias.DTOs;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderRegisterTests
    {
        [Fact]
        public async void Register_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var list = new List<SocialMediaEntry>();
            list.Add(new SocialMediaEntry
            {
                Link = "link.com",
                Type = "link"
            });
            var command = new CreateEventProviderCommand
            {
                Bio = "testt bio",
                Name = "mostafa",
                Password = "facad",
                Email = "hamada@yahoo.com",
                SocialMedias = list
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Register(command);

            Assert.IsType<CreatedResult>(actualResponse as CreatedResult);
        }
        [Fact]
        public async void Create_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            var list = new List<SocialMediaEntry>();
            list.Add(new SocialMediaEntry
            {
                Link = "link.com",
                Type = "link"
            });
            var command = new CreateEventProviderCommand
            {
                Email = "hamada@yahoo.com",
                SocialMedias = list
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeInvalidInputFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Register(command);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }
        [Fact]
        public async void Delete_DoesntExistResponse()
        {
            using var mock = AutoMock.GetLoose();
            var list = new List<SocialMediaEntry>();
            list.Add(new SocialMediaEntry
            {
                Link = "link.com",
                Type = "link"
            });
            var command = new CreateEventProviderCommand
            {
                Bio = "testt bio",
                Name = "mostafa",
                Password = "facad",
                Email = "hamada@yahoo.com",
                SocialMedias = list
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.Register(command))
                .Returns(Task.FromResult(FakeDoesntExistFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.Register(command);
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
