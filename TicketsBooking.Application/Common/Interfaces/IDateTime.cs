using System;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace TicketsBooking.Application.Common.Interfaces
{
    [SingletonService]
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
