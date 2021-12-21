using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketsBooking.UnitTest.AuthTests
{
    public class EventProviderAuthTests
    {
        public EventProviderAuthTests()
        {
        }

        [Fact]
        public void Authenticate_success()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var authCreds = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = "123456789aH",
                };

                var eventProvider = new EventProvider
                {
                    Name = "Test org",
                    Email = "test@test.com",
                };

                var fakeToken = "123456789";

                //Act
                mock.Mock<IEventProviderRepo>()
                    .Setup(e => e.GetEventProvider(authCreds.Email))
                    .Returns(eventProvider);

                mock.Mock<ITokenManager>()
                    .Setup(t => t.GenerateToken(eventProvider, Roles.EventProvider))
                    .Returns(fakeToken);

                var eventProviderService = mock.Create<EventProviderService>();
                var token = eventProviderService.Authenticate(authCreds);

                //Assert
                mock.Mock<IEventProviderRepo>()
                    .Verify(x => x.GetEventProvider(authCreds.Email), Times.Once);

                mock.Mock<ITokenManager>()
                    .Verify(t => t.GenerateToken(eventProvider, Roles.EventProvider), Times.Once);

                Assert.NotNull(token);
                Assert.NotEmpty(token);
                Assert.Equal(fakeToken, token);
            }
        }

        [Fact]
        public void Authenticate_failure()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var authCreds = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = "123456789aH",
                };

                var eventProvider = new EventProvider
                {
                    Name = "Test org",
                    Email = "test@test.com",
                };

                var fakeToken = "123456789";

                //Act
                mock.Mock<IEventProviderRepo>()
                    .Setup(e => e.GetEventProvider(authCreds.Email))
                    .Returns(eventProvider);

                mock.Mock<ITokenManager>()
                    .Setup(t => t.GenerateToken(eventProvider, Roles.EventProvider))
                    .Returns(fakeToken);

                var eventProviderService = mock.Create<EventProviderService>();
                var token = eventProviderService.Authenticate(authCreds);

                //Assert
                mock.Mock<IEventProviderRepo>()
                    .Verify(x => x.GetEventProvider(authCreds.Email), Times.Once);

                mock.Mock<ITokenManager>()
                    .Verify(t => t.GenerateToken(eventProvider, Roles.EventProvider), Times.Once);

                Assert.NotNull(token);
                Assert.NotEmpty(token);
                Assert.Equal(fakeToken, token);
            }
        }
    }
}
