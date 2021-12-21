using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderRepo
    {
        Task<bool> Register(RegisterOrgCommand command);
        Task<bool> DoesOrgAlreadyExist(string Name);
        Task<EventProvider> GetEventProvider(string email);
    }
}