using System.Threading.Tasks;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace TicketsBooking.Infrastructure.Repos
{
    public class EventProviderRepo : IEventProviderRepo
    {
        private readonly AppDbContext _dbContext;

        public EventProviderRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Delete(string name)
        {
            var entity = _dbContext.EventProviders.Find(name);
            _dbContext.EventProviders.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<EventProvider>> GetAll(GetAllEventProvidersQuery query)
        {
            var result = _dbContext.EventProviders.AsQueryable();

            string searchTarget = query.searchTarget;
            if (!string.IsNullOrEmpty(searchTarget))
            {
                result = result.Where(record => record.Name.Contains(searchTarget));
            }
            result = result.Where(record => record.Verified == query.isVerified);
            result = result.Skip((query.pageNumber - 1) * query.pageSize).Take(query.pageSize);
            return await result.ToListAsync();
        }

        public async Task<EventProvider> GetSingleByName(string name)
        {
            return await _dbContext.EventProviders.FindAsync(name.ToLower());
        }

        public async Task<bool> UpdateVerified(SetVerifiedCommand command)
        {
            var result = _dbContext.EventProviders.Find(command.Name.ToLower());
            result.Verified = command.Verified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<EventProvider> Create(CreateEventProviderCommand command)
        {
            var newEventProvider = new EventProvider
            {
                Name = command.Name.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(command.Password),
                Email = command.Email.ToLower(),
                Bio = command.Bio,
                WebsiteLink = command.WebsiteLink,
                Verified = false,
            };

            if (command.SocialMedias != null)
            {
                newEventProvider.SocialMedias = new List<SocialMedia>();
                foreach (var entry in command.SocialMedias)
                {
                    if (entry.Link == null) continue;
                    var newSocialMedia = new SocialMedia
                    {
                        Type = entry.Type,
                        Link = entry.Link,
                    };
                    newEventProvider.SocialMedias.Add(newSocialMedia);
                }
            }

            await _dbContext.EventProviders.AddAsync(newEventProvider);
            await _dbContext.SaveChangesAsync();

            return newEventProvider;
        }

        public async Task<EventProvider> GetSingleByEmail(string email)
        {
            return await _dbContext.EventProviders.FirstOrDefaultAsync(eventProvider => eventProvider.Email == email.ToLower());
        }
    }
}