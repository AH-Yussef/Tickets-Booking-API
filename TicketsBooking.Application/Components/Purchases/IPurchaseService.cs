using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases.DTOs.Results;

namespace TicketsBooking.Application.Components.Purchases
{
    [ScopedService]
    public interface IPurchaseService
    {
        public Task<OutputResponse<PurchaseSingleResult>> CreateNewPurchase(CreateNewPurchaseCommand command);
        public Task<OutputResponse<PurchaseSingleResult>> GetSingle(string purchaseID);
        public Task<OutputResponse<List<PurchaseSingleResult>>> GetAllNotPassed(string customerID);
        public Task<OutputResponse<List<PurchaseSingleResult>>> GetAllPassed(string customerID);
        public Task<OutputResponse<bool>> Refund(string purchaseID);

    }
}
