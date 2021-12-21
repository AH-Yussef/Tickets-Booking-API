using System;
using System.Collections.Generic;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Commands
{
    public class RegisterOrgCommand
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string WebsiteLink { get; set; }
        public List<SocialMediaEntry> SocialMedias { get; set; }

        public class SocialMediaEntry
        {
            public string Type { get; set; }
            public string Link { get; set; }
        }
    }
}
