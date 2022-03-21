using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using TicketsBooking.Application.Components.EventProviders;

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventProviderTests
{
    public class EventProviderDeleteTests
    {
        [Fact]
        public async void Delete_RecordExists()
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
                .Setup(repo => repo.Delete(fakeName))
                .Returns(Task.FromResult(true));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
            //Act
            var actualResponse = await eventProviderService.Decline(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.Delete(fakeName), Times.Once);

            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_RecordDoesntExists()
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
                .Setup(repo => repo.Delete(fakeName))
                .Returns(Task.FromResult(false));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult((EventProvider)null));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
            };
            //Act
            var actualResponse = await eventProviderService.Decline(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.Delete(fakeName), Times.Never);

            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_NullInput()
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
                .Setup(repo => repo.Delete(fakeName))
                .Returns(Task.FromResult(false));


            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
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
