using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.Events.DTOs.Commands
{
    public class SetAcceptedCommand
    {
        public string ID { get; set; }
        public bool Accepted { get; set; }
    }
}
