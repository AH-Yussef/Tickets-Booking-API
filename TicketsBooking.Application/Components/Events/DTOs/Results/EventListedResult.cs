using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Results
{
    public class EventListedResult : IMapFrom<Event>
    {
        public string EventID { get; set; }
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public DateTime DateTime { get; set; }
        public int AllTickets { get; set; }
        public int SingleTicketPrice { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventListedResult, Event>().ReverseMap();
        }
    }
}
