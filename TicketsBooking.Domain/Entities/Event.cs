using System;
using System.Collections.Generic;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Event : EditableEntity
    {
        public EventProvider Provider { get; set; }
        public string EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int AllTickets { get; set; }
        public int BoughtTickets { get; set; }
        public DateTime ReservationDueDate { get; set; }
        public string Location { get; set; }
        public Tag Category { get; set; }
        public Tag SubCategory { get; set; }
        public bool Accepted { get; set; }
        public ICollection<EventTag> Tags { get; set; }
        public ICollection<Participant> participants { get; set; }


    }
}
