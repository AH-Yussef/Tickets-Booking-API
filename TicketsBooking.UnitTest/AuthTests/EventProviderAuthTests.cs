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
using TicketsBooking.Infrastructure.Repos;
using Xunit;

namespace TicketsBooking.UnitTest.AuthTests
{
    public class EventProviderAuthTests
    {
        public EventProviderAuthTests()
        {
        }

        [Fact]
        public void Authenticate()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var authCreds = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = "123",
                };

                mock.Mock<IEventProviderRepo>()
                    .Setup(x => x.GetEventProvider("test@test.com"))
                    .Returns(new EventProvider { Name = "The LoL" });

                var eventProviderService = mock.Create<EventProviderService>();
                eventProviderService.Authenticate(authCreds);

                mock.Mock<IEventProviderRepo>()
                    .Verify(x => x.GetEventProvider("test@test.com"), Times.Once);
            }
        }
    }
}
