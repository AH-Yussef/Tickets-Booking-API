using System;
using System.ComponentModel.DataAnnotations;

namespace TicketsBooking.Application.Components.Authentication
{
    public class AuthCreds
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
