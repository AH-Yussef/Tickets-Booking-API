using System;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Results
{
    public class EventResultListed : IMapFrom<Event>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Place { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventResultListed, Event>()
                .ReverseMap();
        }
    }
}