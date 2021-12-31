using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Customers.DTOs.Results
{
    public class GetSingleUserResult
    {
        public string Name { get; set; }
        public string Email { get; set; }
        //public List<string> Intrests { get; set; }
    }
}
