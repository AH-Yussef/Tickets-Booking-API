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
            private const string Prefix = Rule + "Events/";
            public const string List = Prefix;
            public const string Create = Prefix + "Create";
            public const string Delete = Prefix + "Delete";
            public const string GetAll = Prefix + "GetAll";
            public const string GetSingle = Prefix + "GetSingle";
            public const string Accept = Prefix + "Accept";
            public const string Decline = Prefix + "Decline";
            public const string Filter = Prefix + "Filter";
            public const string Search = Prefix + "Search";
            public const string GetNearlyFinished = Prefix + "GetNearlyFinished";

        }

        public static class EventProvider
        {
            private const string Prefix = Rule + "EventProviders/";
            public const string Register = Prefix + "Register";
            public const string EventProviderAlreadyExists = Prefix + "EventProviderAlreadyExist";
            public const string GetAll = Prefix + "GetAll";
            public const string GetSingle = Prefix + "GetSingle";
            public const string UpdateEventProvider = Prefix + "UpdateEventProvider";
            public const string Delete = Prefix + "Delete";
            public const string SetVerified = Prefix + "SetVerified";
            public const string Auth = Prefix + "Auth";
            public const string Approve = Prefix + "Approve";
            public const string Decline = Prefix + "Decline";
        }
        
        public static class Admin
        {
            private const string Prefix = Rule + "Admin/";
            public const string Auth = Prefix + "AuthAdmin";
        }

        public static class Customer
        {
            private const string Prefix = Rule + "Customer/";
            public const string Auth = Prefix + "AuthUser";
            public const string Register = Prefix + "Register";
            public const string GetAll = Prefix + "GetAll";
            public const string GetSingle = Prefix + "GetSingle";
            public const string Approve = Prefix + "Approve";
            public const string Delete = Prefix + "Delete";
        }
        public static class Purchase
        {
            private const string Prefix = Rule + "Purchases/";
            public const string List = Prefix;
            public const string Create = Prefix + "Create";
            public const string GetSingle = Prefix + "GetSingle";
            public const string GetAllNotPassed = Prefix + "GetAllNotPassed";
            public const string Refund = Prefix + "Refund";


        }
    }
}
