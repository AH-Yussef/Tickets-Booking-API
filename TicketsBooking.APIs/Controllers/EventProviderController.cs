using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;

namespace TicketsBooking.APIs.Controllers
{
    public class EventProviderController: CoreController
    {
        private readonly IEventProviderService _eventProviderService;
        public EventProviderController(IEventProviderService eventProviderService)
        {
            _eventProviderService = eventProviderService;
        }

        
        [AllowAnonymous]
        [HttpPost(Router.EventProvider.Register)]
        public async Task<IActionResult> Register([FromForm] RegisterOrgCommand command)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost(Router.EventProvider.Auth)]
        public string Authenticate([FromBody] AuthCreds creds)
        {
            var result = _eventProviderService.Authenticate(creds);
            return result;
        }


        [AllowAnonymous]
        [HttpGet(Router.EventProvider.OrgAlreadyExist)]
        public async Task<IActionResult> DoesOrgAlreadyExist([FromQuery] DoesOrgAlreadyExistQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
