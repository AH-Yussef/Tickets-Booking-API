using System;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Participant : EditableEntity
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string ParticipantID { get; set; }
        public string EventID { get; set; }
        public string ImageURL { get; set; }
        public string type  { get; set; }

    }
}
