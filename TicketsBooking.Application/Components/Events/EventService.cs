using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Crosscut.Constants;

namespace TicketsBooking.Application.Components.Events
{
    public class EventService: IEventService
    {
        private readonly IEventRepo _eventRepo;
        private readonly IMapper _mapper;

        public EventService(IEventRepo eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<OutputResponse<List<EventResultListed>>> List(ListEventsQuery query)
        {
            var events = await _eventRepo.ListAsync(query);
            var result = _mapper.Map<List<EventResultListed>>(events);
            return new OutputResponse<List<EventResultListed>>
            {
                Message = ResponseMessages.Success,
                Success = true,
                Model = result,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
