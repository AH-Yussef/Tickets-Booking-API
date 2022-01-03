using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Tag
    {
        public string Keyword { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
