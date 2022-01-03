using System;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Participants.DTOs
{
    public class ParticipantEntry : IMapFrom<Participant>
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ParticipantEntry, Event>().ReverseMap();
        }
    }
}
