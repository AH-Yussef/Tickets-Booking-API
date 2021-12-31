using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderRepo
    {
        Task<bool> UpdateVerified(SetVerifiedCommand command);
        Task<EventProvider> Create(CreateEventProviderCommand command);
        Task<EventProvider> GetSingleByName(string name);
        Task<EventProvider> GetSingleByEmail(string email);
        Task<bool> Delete(string name);
        Task<List<EventProvider>> GetAll(GetAllEventProvidersQuery query);
    }
}