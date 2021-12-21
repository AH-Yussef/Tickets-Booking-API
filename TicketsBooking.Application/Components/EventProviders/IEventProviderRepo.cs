using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
namespace TicketsBooking.Application.Components.EventProviders
{
    [ScopedService]
    public interface IEventProviderRepo
    {
        Task<bool> UpdateEventProvider(RegisterOrgCommand command);
        Task<bool> Register(RegisterOrgCommand command);
        Task<bool> DoesOrgAlreadyExist(string name);
        Task<GetSingleQuery> GetSingle(string str);
        Task<bool> Delete(string str);
        Task<List<GetAllQuery>> GetAll(GetAllEventProvidersQuery query);
        Task<bool> SetVerdict(VerdictCommand command);
    }
}