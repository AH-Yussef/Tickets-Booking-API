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
    public class EventGetAllTests
    {
        [Fact]
        public async void GetAll_RecordsExistsNoTarget()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = "testid";


            Event eventRecord = new Event
            {
                EventID = "testid",
                Tags = new List<Tag>(),
            };

            var elr = new EventListedResult
            {
                EventID = testID,
                
                // Tags = new List<string>(),
            };
            var gaeq = new GetAllEventsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Accepted = true,
            };

            List<EventListedResult> elr_l = new List<EventListedResult>();
            elr_l.Add(elr);
            List<Event> list = new List<Event>();
            list.Add(eventRecord);

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetAll(gaeq))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventListedResult>>(list))
                .Returns(elr_l);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = elr_l,
            };
            //Act
            var actualResponse = await eventService.GetAll(gaeq);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetAll(gaeq), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].EventID, expectedResponse.Model[0].EventID);
        }
        [Fact]
        public async void GetAll_RecordsDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = "testid";


            Event eventRecord = new Event
            {
                EventID = "testid",
                Tags = new List<Tag>(),
            };

            var elr = new EventListedResult
            {
                EventID = testID,
                
                // Tags = new List<string>(),
            };
            var gaeq = new GetAllEventsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Accepted = true,
                Query = "testid"
            };

            List<EventListedResult> elr_l = new List<EventListedResult>();
            //elr_l.Add(elr);
            List<Event> list = new List<Event>();
            //list.Add(eventRecord);

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetAll(gaeq))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventListedResult>>(list))
                .Returns(elr_l);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = new List<EventListedResult>(),
            };
            //Act
            var actualResponse = await eventService.GetAll(gaeq);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetAll(gaeq), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Count, expectedResponse.Model.Count);
        }
        [Fact]
        public async void GetAll_RecordsExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            string testID = "testid";


            Event eventRecord = new Event
            {
                EventID = "testid",
                Accepted = true,
                Tags = new List<Tag>(),
            };

            var elr = new EventListedResult
            {
                EventID = testID,
                
                // Tags = new List<string>(),
            };
            var gaeq = new GetAllEventsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Accepted = true,
                Query = "testid"
            };

            List<EventListedResult> elr_l = new List<EventListedResult>();
            elr_l.Add(elr);
            List<Event> list = new List<Event>();
            list.Add(eventRecord);

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetAll(gaeq))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventListedResult>>(list))
                .Returns(elr_l);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = elr_l,
            };
            //Act
            var actualResponse = await eventService.GetAll(gaeq);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetAll(gaeq), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].EventID, expectedResponse.Model[0].EventID);
        }
        [Fact]
        public async void GetAll_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            //string testID = "testid";


            Event eventRecord = new Event
            {
                EventID = "testid",
                Accepted = true,
                Tags = new List<Tag>(),
            };

            //var elr = new EventListedResult
          //  {
          //      ID = "testid",

                // Tags = new List<string>(),
          //  };
            var gaeq = new GetAllEventsQuery
            {
                PageNumber = 1,

                Accepted = true,
               // Query = "testid"
            };

            List<EventListedResult> elr_l = new List<EventListedResult>();
            //elr_l.Add(elr);
            List<Event> list = new List<Event>();
            //list.Add(eventRecord);

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetAll(gaeq))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventListedResult>>(list))
                .Returns(elr_l);

            var eventService = mock.Create<EventService>();

            var expectedResponse = new OutputResponse<List<EventListedResult>>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity
            };
            //Act
            var actualResponse = await eventService.GetAll(gaeq);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetAll(gaeq), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    }
}
