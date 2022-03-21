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
    public class EventSearchTests
    {
        [Fact]
        public async Task EventSearchFull()
        {
            using var mock = AutoMock.GetLoose();
            string query = "cat1";
            Event e = new Event
            {
                EventID = "e",
                Category = "cat1"
            };
            var elr = new EventListedResult
            {
                EventID = "e",
                Category = "cat1",
            };
            List<EventListedResult> elr_l = new List<EventListedResult>();
            elr_l.Add(elr);
            List<Event> list = new List<Event>();
            list.Add(e);
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Search(query))
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
            var actualResponse = await eventService.Search(query);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Search(query), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Category, expectedResponse.Model[0].Category);
        }
        [Fact]
        public async Task EventSearchEmpty()
        {
            using var mock = AutoMock.GetLoose();
            string query = "cat1";
            Event e = new Event
            {
                EventID = "e",
                Category = "cat1"
            };
            var elr = new EventListedResult
            {
                EventID = "e",
                Category = "cat1",
            };
            List<EventListedResult> elr_l = new List<EventListedResult>();
            //elr_l.Add(elr);
            List<Event> list = new List<Event>();
            //list.Add(e);
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Search(query))
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
            var actualResponse = await eventService.Search(query);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Search(query), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Count, expectedResponse.Model.Count);
        }
    }
}
