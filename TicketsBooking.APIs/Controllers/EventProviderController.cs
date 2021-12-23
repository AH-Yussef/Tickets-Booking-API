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
        public async Task<IActionResult> Register([FromForm] CreateEventProviderCommand command)
        {
            var result = await _eventProviderService.Register(command);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.EventProviderAlreadyExists)]
        public async Task<IActionResult> DoesOrgAlreadyExist([FromQuery] string name)
        {
            var result = await _eventProviderService.DoesEventProviderAlreadyExist(name);
            return NewResult(result);
        }
        [AllowAnonymous]
        [HttpGet(Router.EventProvider.Delete)]
        public async Task<IActionResult> Delete([FromQuery] string name)
        {
            var result = await _eventProviderService.Delete(name);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.GetAll)]
        public async Task<IActionResult> GetAll([FromBody] GetAllEventProvidersQuery query)
        {
            var result = await _eventProviderService.GetAll(query);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.GetSingle)]
        public async Task<IActionResult> GetSingle([FromQuery] string name)
        {
            var result = await _eventProviderService.GetSingle(name);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpGet(Router.EventProvider.SetVerified)]
        public async Task<IActionResult> SetVerified([FromQuery] SetVerifiedCommand command)
        {
            var result = await _eventProviderService.UpdateVerified(command);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpPost(Router.EventProvider.Auth)]
        public async Task<IActionResult> Authenticate([FromBody] AuthCreds authCreds)
        {
            var result = await _eventProviderService.Authenticate(authCreds);
            return NewResult(result);
        }
    }
}
