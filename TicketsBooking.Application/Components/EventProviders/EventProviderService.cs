using System;
using System.Threading.Tasks;
using AutoMapper;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Crosscut.Enums;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.EventProviders
{
    public class EventProviderService: IEventProviderService
    {
        private readonly IEventProviderRepo _eventProviderRepo;
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;

        public EventProviderService(IEventProviderRepo eventProviderRepo, ITokenManager tokenManager)
        {
            _eventProviderRepo = eventProviderRepo;
            _tokenManager = tokenManager;
        }

        public Task<bool> DoesOrgAlreadyExist(DoesOrgAlreadyExistQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(RegisterOrgCommand command)
        {
            throw new NotImplementedException();
        }

        public string Authenticate(AuthCreds creds)
        {
            var eventProvider = _eventProviderRepo.GetEventProvider("test@test.com");

            if (eventProvider != null && creds.Password == "123456789aH")
                return _tokenManager.GenerateToken(eventProvider, Roles.EventProvider);
            return null;

            //throw new NotImplementedException();
        }
    }
}
