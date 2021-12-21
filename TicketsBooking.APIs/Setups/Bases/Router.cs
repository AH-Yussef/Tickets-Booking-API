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
            public const string Register = Prefix + "Register"; // ???zzzzzzzzz
            public const string OrgAlreadyExist = Prefix + "OrgAlreadyExist";
            public const string GetAll = Prefix + "GetAll";
            public const string GetSingle = Prefix + "GetSingle";
            public const string UpdateEventProvider = Prefix + "UpdateEventProvider";
            public const string Delete = Prefix + "Delete";
            public const string SetVerdict = Prefix + "SetVerdict";




        }
    }
}
