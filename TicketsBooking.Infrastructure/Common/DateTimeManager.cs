using System;
using TicketsBooking.Application.Common.Interfaces;

namespace TicketsBooking.Infrastructure.Common
{
    public class DateTimeManager : IDateTime
    {
        public DateTime Now { get; } = DateTime.UtcNow;
    }
}
