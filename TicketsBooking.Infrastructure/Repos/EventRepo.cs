using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using Microsoft.EntityFrameworkCore;

namespace TicketsBooking.Infrastructure.Repos
{
    public class EventRepo : IEventRepo
    {
        private readonly AppDbContext _dbContext;

        public EventRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Event>> ListAsync(ListEventsQuery query)
        {
            var result = _dbContext.Events.AsQueryable();

            if (!string.IsNullOrEmpty(query.Q))
            {
                result = result.Where(record =>
                    record.Title.Contains(query.Q) ||
                    record.Details.Contains(query.Q)
                );
            }

            result = result.Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit);

            return await result.ToListAsync();
        }
    }
}