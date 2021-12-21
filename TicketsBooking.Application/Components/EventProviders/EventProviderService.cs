using System;
using System.Collections.Generic;
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

        public Task<bool> DeleteEventProvider(string name)
        {
            return _eventRepo.Delete(name);
            //throw new NotImplementedException();
        }

        public Task<bool> DoesOrgAlreadyExist(DoesOrgAlreadyExistQuery query)
        {
            return _eventRepo.DoesOrgAlreadyExist(query.Name);
            //throw new NotImplementedException();
        }

        public Task<List<GetAllQuery>> GetAll(GetAllEventProvidersQuery query)
        {
            return _eventRepo.GetAll(query);
            //throw new NotImplementedException();
        }

        public Task<GetSingleQuery> GetSingle(string str)
        {
            return _eventRepo.GetSingle(str);
            //throw new NotImplementedException();
        }

        public Task<bool> Register(RegisterOrgCommand command)
        {
           // DoesOrgAlreadyExistQuery doaeq = new DoesOrgAlreadyExistQuery();
            // doaeq.Name = command.Name;
            
            return _eventRepo.Register(command);
            //throw new NotImplementedException();
        }

        public Task<bool> SetVerdict(VerdictCommand command)
        {
            return _eventRepo.SetVerdict(command);
            //throw new NotImplementedException();
        }

        public Task<bool> UpdateEventProvider(RegisterOrgCommand eventProviderInfo)
        {
            return _eventRepo.UpdateEventProvider(eventProviderInfo);
            //throw new NotImplementedException();
        }
    }
}
