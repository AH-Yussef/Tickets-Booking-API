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
    public class EventProviderGetSingleTests
    {
        [Fact]
        public async void GetSingle_RecordExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = fakeEventProviderDTO,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Name, expectedResponse.Model.Name);
        }

        [Fact]
        public async void GetSingle_RecordDoesNotExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult((EventProvider)null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>(); //return null

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = null,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetSingle_NullInput()
        {
            using var mock = AutoMock.GetLoose();

            //Arange
            string fakeName = null;
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = null,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

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
