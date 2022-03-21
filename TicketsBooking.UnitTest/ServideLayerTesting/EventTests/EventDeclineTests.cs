using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.EventProviders;

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventTests
{
    public class EventDeclineTests
    {
        [Fact]
        public async void Delete_RecordExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            EventProvider eventProvider = new EventProvider
            {
                Name = "eventProv",
                Email = "eventProv@gmail.com"
            };
            var testID = "testid";
            var eventInstance = new Event
            {
                EventID = "testID",
                Provider = eventProvider
            };
            var eventDTO = new EventSingleResult
            {
                EventID = eventInstance.EventID,
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Delete(testID))
                .Returns(Task.FromResult(true));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(testID))
                .Returns(Task.FromResult(eventInstance));

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
            //Act
            SetAcceptedCommand sac = new SetAcceptedCommand
            {
                ID = testID,
                Accepted = false
            };
            var actualResponse = await eventService.Decline(testID);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Delete(testID), Times.Once);

            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_RecordDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            EventProvider eventProvider = new EventProvider
            {
                Name = "eventProv",
                Email = "eventProv@gmail.com"
            };
            var testID = "testid";
            var eventInstance = new Event
            {
                EventID = "testID",
                Provider = eventProvider
            };
            var eventDTO = new EventSingleResult
            {
                EventID = eventInstance.EventID,
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Delete(testID))
                .Returns(Task.FromResult(true));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(testID))
                .Returns(Task.FromResult((Event)null));

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
            };
            //Act
            SetAcceptedCommand sac = new SetAcceptedCommand
            {
                ID = testID,
                Accepted = false
            };
            var actualResponse = await eventService.Decline(testID);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Delete(testID), Times.Never);

            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            EventProvider eventProvider = new EventProvider
            {
                Name = "eventProv",
                Email = "eventProv@gmail.com"
            };
            string testID = null;
            var eventInstance = new Event
            {
                EventID = "testID",
                Provider = eventProvider
            };
            var eventDTO = new EventSingleResult
            {
                EventID = eventInstance.EventID,
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Delete(testID))
                .Returns(Task.FromResult(false));

           // mock.Mock<IEventRepo>()
             //   .Setup(repo => repo.GetSingle(testID))
               // .Returns(Task.FromResult((Event)null));

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            SetAcceptedCommand sac = new SetAcceptedCommand
            {
                ID = testID,
                Accepted = false
            };
            var actualResponse = await eventService.Decline(testID);

            //Assert
           // mock.Mock<IEventRepo>()
          //      .Verify(repo => repo.Delete(testID), Times.Never);

            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }

    }
}
