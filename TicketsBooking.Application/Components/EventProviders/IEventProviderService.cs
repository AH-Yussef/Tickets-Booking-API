using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;

namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderService
    {
        Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds);
        Task<OutputResponse<bool>> Register(CreateEventProviderCommand command);
        Task<OutputResponse<bool>> DoesEventProviderAlreadyExist(string query);
        Task<OutputResponse<bool>> Decline(string name);
        Task<OutputResponse<List<EventProviderListedResult>>> GetAll(GetAllEventProvidersQuery query);
        Task<OutputResponse<EventProviderSingleResult>> GetSingle(string name);
        Task<OutputResponse<bool>> Approve(SetVerifiedCommand command);
    }
}
