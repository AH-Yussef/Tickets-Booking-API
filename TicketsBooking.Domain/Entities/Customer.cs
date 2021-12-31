using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Domain.Entities
{
    public class Customer : User
    {
        //public List<string> Intrests { get; set; }
        public bool Accepted { get; set; }
    }
}
