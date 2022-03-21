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
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventAPITests
{
    public class EventSearchTests
    {
        [Fact]
        public async void SearchSuccess()
        {
            using var mock = AutoMock.GetLoose();
            string query = "q1";

            mock.Mock<IEventService>()
                .Setup(service => service.Search(query))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventController = mock.Create<EventController>();
            var actualResponse = await eventController.Search(query);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        private OutputResponse<List<EventListedResult>> FakeSuccessOutput => new OutputResponse<List<EventListedResult>>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = new List<EventListedResult>(),
        };

    }
}
