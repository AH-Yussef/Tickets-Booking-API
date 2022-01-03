using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.Customers.DTOs.Query
{
    public class GetAllUsersQuery
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchTarget { get; set; }
    }
}
