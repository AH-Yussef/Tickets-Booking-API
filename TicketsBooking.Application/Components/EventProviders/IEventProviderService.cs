using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;

namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderService
    {
        Task<string> Authenticate(AuthCreds creds);
        Task<bool> Register(RegisterOrgCommand command);
        Task<bool> DoesOrgAlreadyExist(DoesOrgAlreadyExistQuery query);
    }
}
