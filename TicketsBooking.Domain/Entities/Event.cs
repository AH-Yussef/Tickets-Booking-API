using System;
using System.Collections.Generic;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Event : EditableEntity
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public string Place { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}
