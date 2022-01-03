using System;
using System.Collections.Generic;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Event
    {
        public EventProvider Provider { get; set; }
        public string EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime dateTime { get; set; }
        public int AllTickets { get; set; }
        public float SingleTicketPrice { get; set; }
        public int BoughtTickets { get; set; }
        public DateTime ReservationDueDate { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool Accepted { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
