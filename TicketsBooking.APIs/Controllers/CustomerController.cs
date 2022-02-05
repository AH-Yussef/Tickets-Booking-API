using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
using TicketsBooking.Application.Components.Admins;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.Customers.DTOs.Query;

namespace TicketsBooking.APIs.Controllers
{
    public class CustomerController : CoreController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpPost(Router.Customer.Auth)]
        public async Task<IActionResult> Authenticate([FromBody] AuthCreds authCreds)
        {
            var result = await _customerService.Authenticate(authCreds);
            return NewResult(result);
        }
        [AllowAnonymous]
        [HttpPost(Router.Customer.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
        {
            var result = await _customerService.Register(command);
            return NewResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.Customer.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
        {
            var result = await _customerService.GetAll(query);
            return NewResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.Customer.GetSingle)]
        public async Task<IActionResult> GetSingle([FromQuery] string Email)
        {
            var result = await _customerService.GetSingle(Email);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpGet(Router.Customer.Approve)]
        public async Task<IActionResult> Approve([FromQuery] AcceptCustomerCommand command)
        {
            var result = await _customerService.Approve(command);
            return NewResult(result);
        }

        [HttpPost(Router.Customer.Delete)]
        public async Task<IActionResult> Delete([FromQuery] string Email)
        {
            var result = await _customerService.Delete(Email);
            return NewResult(result);
        }
    }
}
