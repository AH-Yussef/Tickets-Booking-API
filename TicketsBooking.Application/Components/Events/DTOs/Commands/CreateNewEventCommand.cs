using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Commands
{
    public  class CreateNewEventCommand
    {
        public string ProviderName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int AllTickets { get; set; }
        public int BoughtTickets { get; set; }
        public DateTime ReservationDueDate { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool Accepted { get; set; }
        public ICollection<Participant> Participants { get; set; } // participant entry
        // list of tags
        // gets tag ( keyword ) and event ( event id )
        public ICollection<string> Tags { get; set; }
    }
}
