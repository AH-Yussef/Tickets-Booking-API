using System.Threading.Tasks;
using TicketsBooking.Infrastructure.Persistence;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Application.Components.SocialMedia.DTOs;
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
        
        public Task<bool> Delete(string str)
        {
            var entity = _dbContext.EventProviders.Find(str);
            if(entity == null) return new Task<bool>(() => false);
            _dbContext.EventProviders.Remove(entity);
            _dbContext.SaveChanges();
            return new Task<bool>(() => true);
            //throw new System.NotImplementedException();
        }

        public Task<bool> DoesOrgAlreadyExist(string Name)
        {
            var entity = _dbContext.EventProviders.Find(Name);
            return new Task<bool>(() => entity != null);

            //throw new System.NotImplementedException();
        }

        public async Task<List<GetAllQuery>> GetAll(GetAllEventProvidersQuery query) // get some info
        {

            var result = _dbContext.EventProviders.AsQueryable();
            result.Where(record => record.Verified == false);
            result = result.Skip((query.pageNumber - 1) * query.pageSize).Take(query.pageSize);
            //return await result.ToListAsync();
            List<GetAllQuery> queryList = new List<GetAllQuery>();
            //for(EventProvider oneResult : result.ToListAsync())
            foreach(EventProvider oneResult in result)
            {
                GetAllQuery gaq = new GetAllQuery();
                gaq.Name = oneResult.Name;
                gaq.Email = oneResult.Email;
                gaq.Verified = oneResult.Verified;
             
                queryList.Add(gaq);
            }
            //throw new System.NotImplementedException();
            return queryList;
        }

        public Task<GetSingleQuery> GetSingle(string str) // get all info
        {
            var entity = _dbContext.EventProviders.Find(str);
            if(entity == null) return null;
            GetSingleQuery query = new GetSingleQuery();
            query.Address = entity.Address;
            query.Name = entity.Name;
            query.Email = entity.Email;
            query.Verified = entity.Verified;
            query.Bio = entity.Bio;
            query.WebsiteLink = entity.WebsiteLink;
            // social media--
            //throw new System.NotImplementedException();
            return new Task<GetSingleQuery>(() => query);
        }

        public Task<bool> SetVerdict(VerdictCommand command)
        {
            var result = _dbContext.EventProviders.Find(command.Name);
            if(result == null) return new Task<bool>(() => false);
            result.Verified = command.Verified;
            _dbContext.SaveChanges();
            return new Task<bool>(() => true);
            //throw new System.NotImplementedException();
        }

        public Task<bool> Register(RegisterOrgCommand command)
        {
            EventProvider eventProvider = new EventProvider();
            eventProvider.Name = command.Name;
            eventProvider.Password = command.Password;
            eventProvider.Email = command.Email;
            eventProvider.Address = command.Address;
            eventProvider.Bio = command.Bio;
            eventProvider.WebsiteLink= command.WebsiteLink;
            eventProvider.Verified = false;
            // ICollection<SocialMediaEntry> sme = new SocialMediaEntry();
            //sme.Type = 
            //eventProvider.SocialMedias = command.SocialMedias;

            _dbContext.EventProviders.AddAsync(eventProvider);
            _dbContext.SaveChanges();

            return new Task<bool>(() => false); // just for testing purposes
            //throw new System.NotImplementedException();
        }

        public Task<bool> UpdateEventProvider(RegisterOrgCommand command)
        {
            // _dbContext.EventProviders.
            var result = _dbContext.EventProviders.Find(command.Name);
            if(result == null) return new Task<bool>(() => false);
            result.Password = command.Password;
            result.Email = command.Email;
            result.Address = command.Address;
            result.Bio = command.Bio;
            result.WebsiteLink = command.WebsiteLink;
            _dbContext.SaveChanges();
            return new Task<bool>(() => true);
            //throw new System.NotImplementedException();
        }
    }
}