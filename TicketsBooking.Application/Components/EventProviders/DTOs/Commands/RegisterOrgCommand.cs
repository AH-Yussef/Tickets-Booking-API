using System;
using System.Collections.Generic;
using TicketsBooking.Application.Components.SocialMedia.DTOs;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Commands
{
    public class RegisterOrgCommand
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string WebsiteLink { get; set; }
        public List<SocialMediaEntry> SocialMedias { get; set; }
    }
}
