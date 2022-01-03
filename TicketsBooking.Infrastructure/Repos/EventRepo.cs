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
            Event newEvent = new Event
            {
                // creating id by concat the provider name with the event name
                EventID = command.ProviderName + command.Title,
                Provider = await _dbContext.EventProviders.FindAsync(command.ProviderName),
                Title = command.Title,
                Description = command.Description,
                AllTickets = command.AllTickets,
                SingleTicketPrice = command.SingleTicketPrice,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Accepted = false,
                // could need more work possibly if custom tags
                //Category = await _dbContext.Tags.FirstOrDefaultAsync(y => y.keyword == command.Category),
                //SubCategory = await _dbContext.Tags.FirstOrDefaultAsync(y => y.keyword == command.SubCategory),
                Category = command.Category,
                SubCategory = command.SubCategory,
                Tags = new List<Tag>(),
                Participants = new List<Participant>()
            };

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine(command.Participants.Count);
            foreach (var participantEntry in command.Participants)
            {
                var attributes = participantEntry.Split('/');
                var name = attributes[0];
                var role = attributes[1];
                var team = attributes[2];
                Participant participant = new Participant
                {
                    Name = name,
                    Role = role,
                    Team = team,
                };
                newEvent.Participants.Add(participant);
            }
            // then add tag
            // source https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-entity-framework-core/
            command.Tags.Add(command.Category);
            command.Tags.Add(command.SubCategory);
            foreach (string tagKeyword in command.Tags)
            {
                //Tag tag = await _dbContext.Tags.FirstOrDefaultAsync(y => y.keyword == s);

                //EventTag et = new EventTag
                //{
                //    EventId = command.ProviderName + command.Title,
                //    Keyword = tag.keyword,
                //    Tag = tag,
                //    Event = newEvent
                //};
                var tagToAdd = _dbContext.Tags.Find(tagKeyword);
                if (tagToAdd == null)
                {
                    tagToAdd = new Tag { Keyword = tagKeyword };
                }
                newEvent.Tags.Add(tagToAdd);
            }

            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();

            return newEvent;
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
                result = result.Where(record => record.EventID.Contains(searchTarget));
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
            return await _dbContext.Events.Where(e => e.EventID == EventID)
                .Include(e => e.Tags)
                .Include(e => e.Provider)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventID == EventID);
        }

        public async Task<bool> UpdateAccepted(SetAcceptedCommand command)
        {
            var result = _dbContext.Events.Find(command.ID.ToLower());
            result.Accepted = command.Accepted;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<Event> Update(UpdateEventCommand command)
        {
            throw new NotImplementedException();

        }
    }
}
