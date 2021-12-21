using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;

namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderService
    {
        Task<bool> Register(RegisterOrgCommand command); // create new event provider
        Task<bool> DoesOrgAlreadyExist(DoesOrgAlreadyExistQuery query); // checks presence

        Task<bool> DeleteEventProvider(string name); // deletes based on given name

        Task<bool> UpdateEventProvider(RegisterOrgCommand eventProviderInfo);
        Task<List<GetAllQuery>> GetAll(GetAllEventProvidersQuery query);
        Task<GetSingleQuery> GetSingle(string str);
        Task<bool> SetVerdict(VerdictCommand command);
    }
}
