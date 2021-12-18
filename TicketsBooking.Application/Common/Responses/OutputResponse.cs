using System.Collections.Generic;
using System.Net;

namespace TicketsBooking.Application.Common.Responses
{
    public class OutputResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Model { get; set; }
        public object Meta { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
