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
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.EventProviderAPITests
{
    public class EventProviderGetAllTests
    {
        [Fact]
        public async void GetAll_OKresponse()
        {
            using var mock = AutoMock.GetLoose();
            var query = new GetAllEventProvidersQuery
            {
                pageNumber = 1,
                pageSize = 10,
                isVerified = false
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.GetAll(query))
                .Returns(Task.FromResult(FakeSuccessOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.GetAll(query);

            Assert.IsType<AcceptedResult>(actualResponse as AcceptedResult);
        }
        [Fact]
        public async void GetAll_NullInput()
        {
            using var mock = AutoMock.GetLoose();
            var query = new GetAllEventProvidersQuery
            {
                pageNumber = 1,
                pageSize = 10,
                isVerified = false
            };

            mock.Mock<IEventProviderService>()
                .Setup(service => service.GetAll(query))
                .Returns(Task.FromResult(FakeNullStringFailOutput));

            var eventProviderController = mock.Create<EventProviderController>();
            var actualResponse = await eventProviderController.GetAll(query);
            Assert.IsType<UnprocessableEntityObjectResult>(actualResponse as UnprocessableEntityObjectResult);
        }

        private OutputResponse<List<EventProviderListedResult>> FakeSuccessOutput => new OutputResponse<List<EventProviderListedResult>>
        {
            Success = true,
            StatusCode = HttpStatusCode.Accepted,
            Message = ResponseMessages.Success,
            Model = new List<EventProviderListedResult>(),
        };
        private OutputResponse<List<EventProviderListedResult>> FakeNullStringFailOutput => new OutputResponse<List<EventProviderListedResult>>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
    }
}
