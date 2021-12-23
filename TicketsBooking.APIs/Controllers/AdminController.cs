using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
using TicketsBooking.Application.Components.Admins;
using TicketsBooking.Application.Components.Authentication;

namespace TicketsBooking.APIs.Controllers
{
    public class AdminController: CoreController
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [HttpPost(Router.Admin.Auth)]
        public async Task<IActionResult> Authenticate([FromBody] AuthCreds authCreds)
        {
            var result = await _adminService.Authenticate(authCreds);
            return NewResult(result);
        }
    }
}
