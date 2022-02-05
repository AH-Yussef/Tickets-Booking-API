using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.EventProviders;

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventTests
{
    public class EventAcceptTests
    {
        [Fact]
        public async void SetVerified_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetAcceptedCommand
            {
                ID = null,
                Accepted = true,
            };
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.UpdateAccepted(fakeDTO))
                .Returns(Task.FromResult(false));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeDTO.ID));

            var eventService = mock.Create<EventService>();
            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };

            // Act
            var actualResponse = await eventService.Accept(null);

            // Assert
            mock.Mock<IEventRepo>()
               .Verify(repo => repo.UpdateAccepted(fakeDTO), Times.Never);

            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeDTO.ID), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void SetVerified_ValidDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetAcceptedCommand
            {
                ID = "testid",
                Accepted = true,
            };
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.UpdateAccepted(fakeDTO))
                .Returns(Task.FromResult(false));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeDTO.ID));

            var eventService = mock.Create<EventService>();
            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = false,
            };

            // Act
            var actualResponse = await eventService.Accept("testid");

            // Assert
            mock.Mock<IEventRepo>()
               .Verify(repo => repo.UpdateAccepted(fakeDTO), Times.Never);

            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeDTO.ID), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void SetVerified_ValidExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetAcceptedCommand
            {
                ID = "testid",
                Accepted = true,
            };
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.UpdateAccepted(fakeDTO))
                .Returns(Task.FromResult(true));

            EventProvider eventProvider = new EventProvider
            {
                Name = "eventProv",
                Email = "eventProv@gmail.com"
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeDTO.ID))
                .Returns(Task.FromResult(new Event
                {
                    EventID = "testid",
                    Accepted = false,
                    Provider = eventProvider
                }));

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };

            // Act
            var actualResponse = await eventService.Accept("testid");

            // Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeDTO.ID), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

    }
}
