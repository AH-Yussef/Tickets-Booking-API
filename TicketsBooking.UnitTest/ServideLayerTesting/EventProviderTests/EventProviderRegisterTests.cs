using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Integration.Email;
using TicketsBooking.Integration.Email.Models;

namespace TicketsBooking.UnitTest
{
    public class EventProviderServiceTests
    {
        [Theory]
        [ClassData(typeof(RegisterInvalidTest))]
        public async void Create_Invalid(CreateEventProviderCommand fakeEventProviderDTO)
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var eventProviderDTO = fakeEventProviderDTO;

            var fakeName = eventProviderDTO.Name;
            var fakeEventProvider = new EventProvider
            {
                Name = fakeEventProviderDTO.Name,
                Password = fakeEventProviderDTO.Password,
                Email = fakeEventProviderDTO.Email,
                Bio = fakeEventProviderDTO.Bio,
                WebsiteLink = fakeEventProviderDTO.WebsiteLink
            };

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = false
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            //Act
            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.Create(fakeEventProviderDTO))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            var eventProviderService = mock.Create<EventProviderService>();

            var actualResponse = await eventProviderService.Register(fakeEventProviderDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Never);
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.Create(fakeEventProviderDTO), Times.Never);
            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void Create_ValidAlreadyExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEventProvider = new EventProvider
            {
                Name = "Lol",
                Password = "dassa",
                Email = "m@test",
                Bio = "event provider repo",
                WebsiteLink = "webLink.com"
            };
            var fakeName = fakeEventProvider.Name;
            var fakeEventProviderDTO = new CreateEventProviderCommand
            {
                Name = fakeEventProvider.Name,
                Password = fakeEventProvider.Password,
                Email = fakeEventProvider.Email,
                Bio = fakeEventProvider.Bio,
                WebsiteLink = fakeEventProvider.WebsiteLink
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.Create(fakeEventProviderDTO))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ResponseMessages.Failure,
                Model = false
            };
            //Act
            var actualResponse = await eventProviderService.Register(fakeEventProviderDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.Create(fakeEventProviderDTO), Times.Never);
            mock.Mock<IMailService>()
                .Verify(mailService => mailService.SendEmailAsync(fakeMailDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void Create_ValidNew()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEventProvider = new EventProvider
            {
                Name = "Lol",
                Password = "dassa",
                Email = "m@test",
                Bio = "event provider repo",
                WebsiteLink = "webLink.com"
            };
            var fakeName = fakeEventProvider.Name;
            var fakeEventProviderDTO = new CreateEventProviderCommand
            {
                Name = fakeEventProvider.Name,
                Password = fakeEventProvider.Password,
                Email = fakeEventProvider.Email,
                Bio = fakeEventProvider.Bio,
                WebsiteLink = fakeEventProvider.WebsiteLink
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.Create(fakeEventProviderDTO))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(fakeName))
                .Returns(Task.FromResult((EventProvider)null));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };
            //Act
            var actualResponse = await eventProviderService.Register(fakeEventProviderDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingleByName(fakeName), Times.Once);
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.Create(fakeEventProviderDTO), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
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