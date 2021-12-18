using System;

namespace TicketsBooking.APIs.Setups.Bases
{
    public static class Router
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Rule = Root + "/" + Version + "/";

        public static class Event
        {
            private const string Prefix = Rule + "Events";
            public const string List = Prefix;
        }

        public static class EventProvider
        {
            private const string Prefix = Rule + "EventProviders";
            public const string Register = Prefix;
            public const string OrgAlreadyExist = Prefix + "OrgAlreadyExist";
        }
    }
}
