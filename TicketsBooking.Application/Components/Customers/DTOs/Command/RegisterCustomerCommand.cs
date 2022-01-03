using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBooking.Application.Components.Customers.DTOs.Command
{
    public class RegisterCustomerCommand
    {
        public string Name { get; set;}
        public string Email { get; set;}
        public string Password { get; set;}
    }
}
