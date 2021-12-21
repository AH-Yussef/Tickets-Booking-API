using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Queries
{
    public class GetAllQuery
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
    }
}
