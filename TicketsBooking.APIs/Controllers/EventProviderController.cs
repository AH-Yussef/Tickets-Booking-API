using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
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
            // missing await, ask ALI
            // call register function from service provider
            return (IActionResult)_eventProviderService.Register(command);
            //throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.OrgAlreadyExist)]
        public async Task<IActionResult> DoesOrgAlreadyExist([FromQuery] DoesOrgAlreadyExistQuery query)
        {
            return (IActionResult)_eventProviderService.DoesOrgAlreadyExist(query);
            //throw new NotImplementedException();
        }
        [AllowAnonymous]
        [HttpGet(Router.EventProvider.Delete)]
        public async Task<IActionResult> DeleteEventProvider([FromQuery] DoesOrgAlreadyExistQuery query)
        {
            return (IActionResult)_eventProviderService.DeleteEventProvider(query.Name);
            //throw new NotImplementedException();
        }


        [AllowAnonymous]
        [HttpGet(Router.EventProvider.GetAll)]
        public async Task<IActionResult> GetAll(GetAllEventProvidersQuery query)
        {
            return (IActionResult)_eventProviderService.GetAll(query);
            //throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.GetSingle)]
        public async Task<IActionResult> GetSingle([FromQuery] DoesOrgAlreadyExistQuery query)
        {
            return (IActionResult)_eventProviderService.GetSingle(query.Name);
            //throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.UpdateEventProvider)]
        public async Task<IActionResult> UpdateEventProvider([FromForm]RegisterOrgCommand command)
        {
            return (IActionResult)_eventProviderService.UpdateEventProvider(command);
            //throw new NotImplementedException();
        }
        [AllowAnonymous]
        [HttpGet(Router.EventProvider.SetVerdict)]
        public async Task<IActionResult> SetVerdict([FromForm]VerdictCommand command)
        {
            return (IActionResult)_eventProviderService.SetVerdict(command);
        }


    }
}
