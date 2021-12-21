using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketsBooking.UnitTest
{
    public class EventProviderServiceTests
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
                .Setup(repo => repo.GetSingle(fakeName))
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
                .Verify(repo => repo.GetSingle(fakeName), Times.Once);

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
                .Setup(repo => repo.GetSingle(fakeName))
                .Returns<EventProvider>(null); //fix

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
                .Verify(repo => repo.GetSingle(fakeName), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Name, expectedResponse.Model.Name);
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
