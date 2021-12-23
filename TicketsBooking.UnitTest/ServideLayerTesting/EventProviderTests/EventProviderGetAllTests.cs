using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;


namespace TicketsBooking.UnitTest.ServideLayerTesting.EventProviderTests
{
    public class EventProviderGetAllTests
    {
        // GetAll tests
        [Fact]
        public async void GetAll_RecordsExistNoSearchTarget()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();
            listFake.Add(fakeEventProviderDTO);

            List<EventProvider> list = new List<EventProvider>();
            list.Add(fakeEventProvider);

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = listFake,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Name, expectedResponse.Model[0].Name);
        }
        [Fact]
        public async void GetAll_InvalidQuery()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = false,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();


            List<EventProvider> list = new List<EventProvider>();


            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));
            //.Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetAll_RecordsDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();


            List<EventProvider> list = new List<EventProvider>();


            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));
            //.Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = new List<EventProviderListedResult>(),
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

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

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();
            listFake.Add(fakeEventProviderDTO);

            List<EventProvider> list = new List<EventProvider>();
            list.Add(fakeEventProvider);

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = listFake,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Name, expectedResponse.Model[0].Name);
        }
        // GetSingle tests


        private List<EventProvider> GetSampleEventProviders()
        {
            var sample = new List<EventProvider>();
            sample.Add(new EventProvider
            {
                Name = "LOL",
            });
            sample.Add(new EventProvider
            {
                Name = "Mostafa",
            });
            sample.Add(new EventProvider
            {
                Name = "Tarek",
            });
            sample.Add(new EventProvider
            {
                Name = "Shosh",
            });

            return sample;
        }
    }
}
