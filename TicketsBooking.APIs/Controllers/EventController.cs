using System.Threading.Tasks;
using TicketsBooking.APIs.Setups.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Crosscut.Enums;

namespace TicketsBooking.APIs.Controllers
{
    public class EventController : CoreController
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        //[AllowAnonymous]
        [Authorize(Roles = "EventProvider")]
        [HttpGet(Router.Event.List)]
        public async Task<IActionResult> List([FromQuery] ListEventsQuery query)
        {
            var result = await _eventService.List(query);
            return NewResult(result);
        }
    }
}
