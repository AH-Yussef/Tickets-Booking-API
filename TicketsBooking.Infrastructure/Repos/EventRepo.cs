using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure.Repos
{
    public class EventRepo : IEventRepo
    {
        private readonly AppDbContext _dbContext;

        public EventRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Event> Create(CreateNewEventCommand command)
        {
            Event eventRelation = new Event
            {
                // creating id by concat the provider name with the event name
                EventID = command.Provider.Name + command.Title,
                Title = command.Title,
                Description = command.Description,
                Created = command.Created,
                AllTickets = command.AllTickets,
                BoughtTickets = command.BoughtTickets,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Category = command.Category,
                SubCategory = command.SubCategory,
                Accepted = command.Accepted,
                participants = command.Participants,
            };

            // create EventTags from the tag given in the command
            List<Tag> tags = (List<Tag>)command.Tags;
            List<EventTag> eventTagList = new List<EventTag>();
            foreach (Tag tag in tags)
            {
                EventTag et = new EventTag
                {
                    EventId = command.Provider.Name + command.Title,
                    keyword = tag.keyword,
                    tag = tag,
                    eventRelation = eventRelation
                };
                eventTagList.Add(et);
            }
            // fill the tags event tag
            eventRelation.Tags = eventTagList;
            await _dbContext.Events.AddAsync(eventRelation);
            await _dbContext.SaveChangesAsync();

            return eventRelation;

        }

        public async Task<bool> Delete(string EventID)
        {
            //throw new NotImplementedException();
            var entity = await _dbContext.Events.FindAsync(EventID);
            //var entity = entities.Find(e => e.EventID == EventID); // ?? not sure

            _dbContext.Events.Remove(entity);

            await _dbContext.SaveChangesAsync();
            // because we used on cascade delete then the appropriate entries will get
            // deleted automatically from EventTag table
            return true;
        }

        public async Task<List<Event>> GetAll(GetAllEventsQuery query)
        {
            var result = _dbContext.Events
                //.Include(e => e.Tags)
                .Include(e => e.Provider)
                .AsQueryable();

            string searchTarget = query.Query;
            if (!string.IsNullOrEmpty(searchTarget))
            {
                result = result.Where(record => record.Title.Contains(searchTarget));
            }
            // pull the events having the same accepted status as the query
            result = result.Where(record => record.Accepted == query.Accepted);
            result = result.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            return await result.ToListAsync();
        }

        public async Task<Event> GetSingle(string EventID)
        {
            // var entities = await _dbContext.Events.Include(e => e.Tags).ToListAsync();// ?? not sure
            //var entity = entities.FirstOrDefault(e => e.EventID == EventID); 
            return await _dbContext.Events
                .Include(e => e.Tags)
                .Include(e => e.Provider)
                .FirstOrDefaultAsync(e => e.EventID == EventID);
        }

        public Task<Event> Update(UpdateEventCommand command)
        {
            throw new NotImplementedException();

        }
    }
}
