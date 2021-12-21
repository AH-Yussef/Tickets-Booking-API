using System.Threading.Tasks;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Repos
{
    public class EventProviderReop : IEventProviderRepo
    {
        private readonly AppDbContext _dbContext;

        public EventProviderReop(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> DoesOrgAlreadyExist(string Name)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Register(RegisterOrgCommand command)
        {
            throw new System.NotImplementedException();
        }

        public EventProvider GetEventProvider(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}