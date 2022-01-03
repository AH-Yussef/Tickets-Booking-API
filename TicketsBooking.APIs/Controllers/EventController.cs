using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Crosscut.Constants;
namespace TicketsBooking.APIs.Controllers
{
    public class EventController : CoreController
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [Authorize(Roles = "EventProvider")]
        [HttpPost(Router.Event.Create)]
        public async Task<IActionResult> Create([FromForm] CreateNewEventCommand command)
        {
            var result = await _eventService.Create(command);
            return NewResult(result);
        }
        [Authorize(Roles = "EventProvider")]
        [HttpPost(Router.Event.Delete)]
        public async Task<IActionResult> Delete([FromQuery] string eventID)
        {
            var result = await _eventService.Delete(eventID);
            return NewResult(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost(Router.Event.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEventsQuery query)
        {
            var result = await _eventService.GetAll(query);
            return NewResult(result);
        }
        [Authorize(Roles = "EventProvider")]
        [HttpPost(Router.Event.GetSingle)]
        public async Task<IActionResult> GetSingle([FromForm] string eventID)
        {
            var result = await _eventService.GetSingle(eventID);
            return NewResult(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost(Router.Event.Accept)]
        public async Task<IActionResult> Accept([FromForm] SetAcceptedCommand command)
        {
            var result = await _eventService.Accept(command);
            return NewResult(result);
        }
        [Authorize(Roles = "EventProvider")]
        [HttpPost(Router.Event.Decline)]
        public async Task<IActionResult> Decline([FromForm] SetAcceptedCommand command)
        {
            var result = await _eventService.Decline(command);
            return NewResult(result);
        }
    }
}
