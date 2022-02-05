using System;
using System.Collections.Generic;

namespace TicketsBooking.Application.Components.Events.DTOs.Commands
{
    public class CreateNewEventCommand
    {
        public string ProviderName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public int AllTickets { get; set; }
        public float SingleTicketPrice { get; set; }
        public DateTime ReservationDueDate { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public ICollection<string> Participants { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
