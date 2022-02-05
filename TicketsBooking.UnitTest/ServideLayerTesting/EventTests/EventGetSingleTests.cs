using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.Events;
using AutoMapper;

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventTests
{
    public class EventGetSingleTests
    {
        [Fact]
        public async void GetSingle_RecordExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = "testid";


            Event eventRecord = new Event
            {
                EventID = "testid",
                Tags = new List<Tag>(),
            };

            var esr = new EventSingleResult
            {
                EventID = testID,
                Tags = new List<string>(),
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(testID))
                .Returns(Task.FromResult(eventRecord));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventSingleResult>(eventRecord))
                .Returns(esr);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<EventSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = esr,
            };
            //Act
            var actualResponse = await eventService.GetSingle(testID);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventSingleResult>(eventRecord), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.EventID, expectedResponse.Model.EventID);
        }
        [Fact]
        public async void GetSingle_RecordDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = "testid";

            Event eventRecord = new Event
            {
                EventID = "testid",
                Tags = new List<Tag>(),
            };

            var esr = new EventSingleResult
            {
                EventID = testID,
                Tags = new List<string>(),
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(testID))
                .Returns(Task.FromResult((Event)null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventSingleResult>(eventRecord))
                .Returns(esr);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<EventSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = null,
            };
            //Act
            var actualResponse = await eventService.GetSingle(testID);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventSingleResult>(eventRecord), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetSingle_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = null;

            Event eventRecord = new Event
            {
                EventID = "testid",
                Tags = new List<Tag>(),
            };

            var esr = new EventSingleResult
            {
                EventID = "testid",
                Tags = new List<string>(),
            };

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(testID))
                .Returns(Task.FromResult(eventRecord));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventSingleResult>(eventRecord))
                .Returns(esr);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<EventSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = null,
            };
            //Act
            var actualResponse = await eventService.GetSingle(testID);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(testID), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventSingleResult>(eventRecord), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    }
}
