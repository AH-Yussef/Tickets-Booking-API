using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Application.Components.Participants.DTOs;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Results
{
    public class EventSingleResult : IMapFrom<Event>
    {
        public string EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProviderName { get; set; }
        public DateTime dateTime { get; set; }
        public int AllTickets { get; set; }
        public int BoughtTickets { get; set; }
        public int SingleTicketPrice { get; set; }
        public DateTime ReservationDueDate { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool Accepted { get; set; }
        public ICollection<ParticipantEntry> Participants { get; set; }
        public ICollection<string> Tags { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventSingleResult, Event>().ReverseMap();
        }
    }
}
