using System;
using Moq;
using TicketsBooking.Application.Components.EventProviders;
using Xunit;

namespace TicketsBooking.UnitTest
{
    public class EventProviderServiceTests
    {
        private readonly Mock<IEventProviderRepo> _eventProviderRepo;
        private readonly IEventProviderService _eventProviderService;
        public EventProviderServiceTests(IEventProviderService eventProviderService)
        {
            _eventProviderRepo = new Mock<IEventProviderRepo>();
            _eventProviderService = eventProviderService;
        }

        [Fact]
        public void EventProviderDoesNOTExist()
        {
            //Arange
            //Act
            //Assert
        }
    }
}
