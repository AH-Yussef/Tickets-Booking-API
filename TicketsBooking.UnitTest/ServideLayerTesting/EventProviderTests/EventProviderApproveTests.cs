using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.EventProviders;

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventProviderTests
{
    public class EventProviderUpdateTests
    {
        [Fact]
        public async void UpdateVerified_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = null,
                Verified = true,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(false));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeDTO.Name));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await eventProviderService.Approve(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Never);

            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeDTO.Name), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void UpdateVerified_ValidDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(false));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeDTO.Name))
                .Returns(Task.FromResult((EventProvider)null));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = false,
            };
            //Act
            var actualResponse = await eventProviderService.Approve(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeDTO.Name), Times.Once);

            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void UpdateVerified_ValidExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true,

            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(true));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeDTO.Name))
                .Returns(Task.FromResult(new EventProvider
                {
                    Name = "LOL",
                    Email = "LOL@test.com",
                })); ;

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
            //Act
            var actualResponse = await eventProviderService.Approve(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeDTO.Name), Times.Once);

            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        // GetAll tests

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
        public class RegisterInvalidTest : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var NullPassword = new CreateEventProviderCommand
                {
                    Name = "Lol",
                    Password = null,
                    Email = "m@test",
                    Bio = "event provider repo",
                    WebsiteLink = "webLink.com"
                };
                var NullName = new CreateEventProviderCommand
                {
                    Name = null,
                    Password = "dassa",
                    Email = "m@test",
                    Bio = "event provider repo",
                    WebsiteLink = "webLink.com"
                };
                var NullEmail = new CreateEventProviderCommand
                {
                    Name = "Lol",
                    Password = "dassa",
                    Email = null,
                    Bio = "event provider repo",
                    WebsiteLink = "webLink.com"
                };
                var NullBio = new CreateEventProviderCommand
                {
                    Name = "Lol",
                    Password = "dassa",
                    Email = "m@test",
                    Bio = null,
                    WebsiteLink = "webLink.com"
                };
                var NullCombination = new CreateEventProviderCommand
                {
                    Name = "Lol",
                    Password = null,
                    Email = null,
                    Bio = null,
                    WebsiteLink = "webLink.com"
                };

                yield return new object[] { NullName };
                yield return new object[] { NullEmail };
                yield return new object[] { NullBio };
                yield return new object[] { NullPassword };
                yield return new object[] { NullCombination };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
