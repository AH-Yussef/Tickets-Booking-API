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
            //if (creds.Email == "test@test.com" && creds.Password == "123") return _tokenManager.GenerateToken(user, Roles.EventProvider);
            //return null;
            _eventProviderRepo.GetEventProvider("test@test.com");
            return "ali";

            //throw new NotImplementedException();
        }
    }
}
