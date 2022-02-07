using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Purchases;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure.Repos
{
    public class PurchaseRepo:IPurchaseRepo
    {
        private readonly AppDbContext _dbContext;

        public PurchaseRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Purchase> CreateNewPurchase(CreateNewPurchaseCommand command)
        {
            Event eventObject = await _dbContext.Events
                .Include(e => e.Purchases)
                .FirstOrDefaultAsync(e => e.EventID == command.EventID);
            
            Customer customer = await _dbContext.Customers
                .Include(e => e.Purchases)
                .FirstOrDefaultAsync(e => e.Email == command.CustomerID);
            
            Purchase purchase = new Purchase
            {
                PurchaseID = command.CustomerID + command.EventID + eventObject.BoughtTickets,
                ReservationDate = command.PurchaseTime,
                TicketsCount = command.TicketsCount,
                SingleTicketCost = eventObject.SingleTicketPrice,
                //customerObject = customer,
                //eventObject = eventObject
            };

            eventObject.Purchases.Add(purchase);
            
            customer.Purchases.Add(purchase);

            eventObject.BoughtTickets += command.TicketsCount;
            await _dbContext.Purchases.AddAsync(purchase);
            await _dbContext.SaveChangesAsync();
            
            return purchase;
            throw new NotImplementedException();

        }

        public Task<List<Purchase>> GetAll(string CustomerID)
        {
            throw new NotImplementedException();
        }

        public async Task<Purchase> GetSingle(string purchaseID)
        {
            /*     return await _dbContext.Purchases
                     .Include(p => p.eventObject)
                     .Include(p => p.customerObject)
                     .FirstOrDefaultAsync(p => p.PurchaseID == purchaseID);*/
            throw new NotImplementedException();
        }
    }
}
