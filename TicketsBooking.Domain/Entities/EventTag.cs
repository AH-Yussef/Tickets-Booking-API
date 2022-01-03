using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Domain.Entities
{
    public class EventTag
    {
        public string EventId { get; set; }
        public string keyword { get; set; }
        public Event eventRelation { get; set; }
        public Tag tag { get; set; }

    }
}
