using System;
namespace TicketsBooking.Application.Components.Customers.DTOs.Command
{
    public class AcceptCustomerCommand
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
