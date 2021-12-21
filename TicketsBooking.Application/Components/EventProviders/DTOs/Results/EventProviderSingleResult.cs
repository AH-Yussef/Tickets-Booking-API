using System.Collections.Generic;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Application.Components.SocialMedia.DTOs;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Results
{
    public class EventProviderSingleResult: IMapFrom<EventProvider>
    {
        public string Name { get; set; }
        public bool Verified { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string WebsiteLink { get; set; }
        public List<SocialMediaEntry> SocialMedias { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventProviderSingleResult, EventProvider>().ReverseMap();
        }
    }
}
