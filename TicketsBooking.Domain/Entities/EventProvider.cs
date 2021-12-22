using System;
using System.Collections.Generic;

namespace TicketsBooking.Domain.Entities
{
    public class EventProvider
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string WebsiteLink { get; set; }
        public bool Verified { get; set; }
        public ICollection<SocialMedia> SocialMedias { get; set; }
    }
}
