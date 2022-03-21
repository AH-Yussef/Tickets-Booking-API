using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.Purchases.DTOs.Commands
{
    public class CreateNewPurchaseCommand
    {
        public string EventID { get; set; }
        public string CustomerID { get; set; }
        public int TicketsCount { get; set; }

    }
}
