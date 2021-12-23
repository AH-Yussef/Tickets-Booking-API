using System;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Results
{
    public class EventProviderListedResult : IMapFrom<EventProvider>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventProviderListedResult, EventProvider>().ReverseMap();
        }
    }
}
