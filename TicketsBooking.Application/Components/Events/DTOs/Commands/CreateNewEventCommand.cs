using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Participants.DTOs;
using TicketsBooking.Domain.Entities;

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
        public ICollection<string> Participants { get; set; } // participant entry of dto type
        // list of tags
        // gets tag ( keyword ) and event ( event id )
        public ICollection<string> Tags { get; set; }
    }
}
