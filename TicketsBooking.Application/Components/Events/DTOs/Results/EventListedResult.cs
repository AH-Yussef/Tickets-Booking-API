using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Results
{
    public class EventListedResult
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public DateTime Created { get; set; }
        public int AllTickets { get; set; }
        public int BoughtTickets { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

    }
}
