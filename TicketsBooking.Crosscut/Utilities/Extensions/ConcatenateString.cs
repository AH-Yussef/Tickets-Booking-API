using System;
using System.Collections.Generic;
using Serilog;

namespace TicketsBooking.Crosscut.Utilities.Extensions
{
    public static class ConcatenateString
    {
        public static ILogger Logger { get; }

        public static string ToConcatenateString(this List<string> items)
        {
            try
            {
                var longString = "";
                var lastIndex = items.Count - 1;
                for (var i = 0; i < lastIndex; i++) longString += items[i] + ",";
                longString += items[lastIndex];
                return longString;
            }
            catch (Exception e)
            {
                Logger.Error(e,"trying to concatenate string");
                return null;
            }
        }
    }
}
