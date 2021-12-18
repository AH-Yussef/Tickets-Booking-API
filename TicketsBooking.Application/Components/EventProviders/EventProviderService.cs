using System;
using System.Threading.Tasks;
using AutoMapper;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;

namespace TicketsBooking.Application.Components.EventProviders
{
    public class EventProviderService: IEventProviderService
    {
        private readonly IEventProviderRepo _eventRepo;
        private readonly IMapper _mapper;

        public EventProviderService(IEventProviderRepo eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public Task<bool> DoesOrgAlreadyExist(DoesOrgAlreadyExistQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(RegisterOrgCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
