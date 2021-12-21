using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.SocialMedia.DTOs;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Queries
{
    public class GetSingleQuery
    {
        public string Name { get; set; }
        public bool Verified { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string WebsiteLink { get; set; }
        public List<SocialMediaEntry> SocialMedias { get; set; }
    }
}
