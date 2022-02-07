using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace TicketsBooking.Application.Components.Purchases
{
    [ScopedService]
    public interface IPurchaseRepo
    {
        public Task<Purchase> CreateNewPurchase(CreateNewPurchaseCommand command);
        public Task<Purchase> GetSingle(string purchaseID);
        public Task<List<Purchase>> GetAll(string CustomerID);
    }
}
