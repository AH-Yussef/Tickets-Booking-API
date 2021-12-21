using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.EventProviders.DTOs.Commands
{
    public class VerdictCommand
    {
        public String Name { get; set; }
        public bool Verified { get; set; }
    }
}
