using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
namespace TicketsBooking.Application.Components.Events
{
    [ScopedService]
    public interface IEventService
    {
        public Task<OutputResponse<bool>> Create(CreateNewEventCommand command);
        public Task<OutputResponse<bool>> Delete(string EventID);
        public Task<OutputResponse<EventSingleResult>> Update(UpdateEventCommand command);
        public Task<OutputResponse<List<EventListedResult>>> GetAll(GetAllEventsQuery query);
        public Task<OutputResponse<List<EventListedResult>>> GetNearlyFinished(int numberOfEventsNeeded);
        public Task<OutputResponse<EventSingleResult>> GetSingle(string EventID);
        public Task<OutputResponse<bool>> Accept(string eventId);
        public Task<OutputResponse<bool>> Decline(string eventId);
        public Task<OutputResponse<List<EventListedResult>>> Filter(string query);
        public Task<OutputResponse<List<EventListedResult>>> Search(string query);
    }
}
