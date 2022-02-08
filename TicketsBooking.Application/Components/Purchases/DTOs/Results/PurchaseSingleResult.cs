using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.Purchases.DTOs.Results
{
    public class PurchaseSingleResult
    {
        public string PurchaseID { get; set; }
        public string EventID { get; set; }
        public string CustomerID { get; set; }
        public DateTime ReservationDate { get; set; }
        public int TicketCount { get; set; }
        public float SingleTicketCost { get; set; }
    }
}
