using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsBooking.Application.Components.Events;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
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
            string eventID = command.ProviderName + command.Title;
            Event newEvent = new Event
            {
                // creating id by concat the provider name with the event name
                EventID = eventID.ToLower(),
                Provider = await _dbContext.EventProviders.FindAsync(command.ProviderName),
                Title = command.Title,
                Description = command.Description,
                AllTickets = command.AllTickets,
                SingleTicketPrice = command.SingleTicketPrice,
                dateTime = command.DateTime,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Accepted = false,
                Category = command.Category,
                SubCategory = command.SubCategory,
                Tags = new List<Tag>(),
                Participants = new List<Participant>()
            };

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

            command.Tags.Add(command.Category);
            command.Tags.Add(command.SubCategory);
            foreach (string tagKeyword in command.Tags)
            {
                var tagToAdd = _dbContext.Tags.Find(tagKeyword.ToLower());
                if (tagToAdd == null)
                {
                    tagToAdd = new Tag { Keyword = tagKeyword.ToLower() };
                }
                newEvent.Tags.Add(tagToAdd);
            }

            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();

            return newEvent;
        }

        public async Task<bool> Delete(string EventID)
        {
            var entity = await _dbContext.Events.FindAsync(EventID);
            _dbContext.Events.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Event>> Filter(string query)
        {
            var result = _dbContext.Events
                .Include(e => e.Provider)
                .Include(e => e.Tags)
                .Where(e => e.Accepted == true)
                .Where(e => DateTime.Compare(e.ReservationDueDate, DateTime.Today) >= 0)
                .OrderBy(e => e.dateTime)
                .AsQueryable();

            string[] tags = query.Split(" "); 
            HashSet<Event> events = new HashSet<Event>();
            foreach (Event e in result)
            {
                foreach (Tag tag in e.Tags)
                {
                    foreach(string queryTag in tags)
                    {
                        if (tag.Keyword == queryTag)
                        {
                            events.Add(e);
                        }
                    }
                }
            }
            var resultList = new List<Event>(events);
           return resultList;
        }

        public async Task<List<Event>> GetAll(GetAllEventsQuery query)
        {
            var result = _dbContext.Events
                .Include(e => e.Provider)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.ProviderName))
            {
                result = result.Where(record => record.Provider.Name.Equals(query.ProviderName));
            }

            string searchTarget = query.Query;
            if (!string.IsNullOrEmpty(searchTarget))
            {
                result = result.Where(record => record.EventID.Contains(searchTarget));
            }

            result = result.Where(record => record.Accepted == query.Accepted);
            result = result.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
            return await result.ToListAsync();
        }

        public async Task<List<Event>> GetNearlyFinished(int numberOfEventsNeeded)
        {
            // gets the events whose bought tickets are >= 90%
            // AKA the remaining is <= 10%
            // and sorts them according to reservation due date

            List<Event> events = await _dbContext.Events
                .AsQueryable()
                .Where(e => e.BoughtTickets >= e.AllTickets * 0.9)
                .OrderBy(e => e.ReservationDueDate)
                .ToListAsync();

            // return the number of events needed
            // if the number required is greater than the size just return the list
            if (numberOfEventsNeeded > events.Count) return events;

            return events.GetRange(0, numberOfEventsNeeded);
            
            throw new NotImplementedException();
        }

        public async Task<Event> GetSingle(string EventID)
        {
            return await _dbContext.Events.Where(e => e.EventID == EventID.ToLower())
                .Include(e => e.Tags)
                .Include(e => e.Provider)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventID == EventID);
        }

        public async Task<List<Event>> Search(string query)
        {
            List<string> queryWords = query.Split(" ").ToList();
            HashSet<Event> result = new HashSet<Event>();
            var events = _dbContext.Events
                .Include(e => e.Provider)
                .Include(e => e.Tags)
                .Where(e => e.Accepted == true)
                .Where(e => DateTime.Compare(e.ReservationDueDate, DateTime.Today) >= 0)
                .OrderBy(e => e.dateTime)
                .AsQueryable();
            foreach (Event e in events)
            {
                List<string> target = new List<string>();
                foreach (Tag tag in e.Tags)
                {
                    target.Add(tag.Keyword.ToLower());
                }
                
                string[] titleWord = e.Title.ToLower().Split(" ");
                
                foreach(string s in titleWord)
                {
                    target.Add(s);
                }
                if (target.Intersect(queryWords).Any())
                {
                    result.Add(e);
                }
            }
            return new List<Event>(result);
        }




        public async Task<bool> UpdateAccepted(SetAcceptedCommand command)
        {
            var result = _dbContext.Events.Find(command.ID.ToLower());
            result.Accepted = command.Accepted;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
