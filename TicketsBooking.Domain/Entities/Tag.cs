using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Common;
namespace TicketsBooking.Domain.Entities
{
    public class Tag : EditableEntity
    {
        public string keyword { get; set; }
        public ICollection<Event> events { get; set; }
    }
}
