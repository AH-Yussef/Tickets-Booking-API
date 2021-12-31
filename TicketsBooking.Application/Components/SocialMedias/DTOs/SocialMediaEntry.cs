using System;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.SocialMedias.DTOs
{
    public class SocialMediaEntry: IMapFrom<SocialMedia>
    {
        public String Type { get; set; }
        public String Link { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SocialMediaEntry, EventProvider>().ReverseMap();
        }
    }
}