using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Events.DTOs.Queries
{
    public class GetAllEventsQuery
    {
        public bool Accepted { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
