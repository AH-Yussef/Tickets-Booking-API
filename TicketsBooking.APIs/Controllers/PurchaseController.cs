using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Setups.Bases;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Purchases;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Crosscut.Constants;
namespace TicketsBooking.APIs.Controllers
{
    public class PurchaseController : CoreController
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        //[Authorize(Roles = "EventProvider,Admin")]
        [AllowAnonymous]
        [HttpPost(Router.Purchase.Create)]
        public async Task<IActionResult> Create([FromForm] CreateNewPurchaseCommand command)
        {
            var result = await _purchaseService.CreateNewPurchase(command);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpPost(Router.Purchase.GetSingle)]
        public async Task<IActionResult> GetSingle([FromQuery] string purchaseID)
        {
            var result = await _purchaseService.GetSingle(purchaseID);
            return NewResult(result);
        }

        [AllowAnonymous]
        [HttpPost(Router.Purchase.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] string customerID)
        {
            var result = await _purchaseService.GetAll(customerID);
            return NewResult(result);
        }

    }
}
