using System.Collections.Generic;
using TicketsBooking.Application.Common.Responses;
using MediatR;
using TicketsBooking.Application.Components.Events.DTOs.Results;

namespace TicketsBooking.Application.Components.Events.DTOs.Queries
{
    public class ListEventsQuery
    {
        public string Q { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}