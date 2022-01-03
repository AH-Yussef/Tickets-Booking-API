using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events
{
    [ScopedService]
    public interface IEventRepo
    {
        Task<Event> Create(CreateNewEventCommand command);
        Task<bool> Delete(string EventID);
        Task<Event> Update(UpdateEventCommand command);
        Task<bool> UpdateAccepted(SetAcceptedCommand command);
        Task<List<Event>> GetAll(GetAllEventsQuery query);
        Task<Event> GetSingle(string EventID);
    }
}
