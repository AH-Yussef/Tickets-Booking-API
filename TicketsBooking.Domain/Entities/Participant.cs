using System;
using TicketsBooking.Domain.Common;

namespace TicketsBooking.Domain.Entities
{
    public class Participant : EditableEntity
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }

    }
}
