using System.Collections.Generic;
using TicketsBooking.Application.Common.Responses;
using MediatR;
using TicketsBooking.Application.Components.Events.DTOs.Results;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Queries
{
    public class DoesOrgAlreadyExistQuery
    {
        public string Name { get; set; }
    }
}