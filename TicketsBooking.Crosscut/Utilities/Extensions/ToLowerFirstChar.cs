namespace TicketsBooking.Crosscut.Utilities.Extensions
{
    public static class LowerFirstChar
    {
        public static string ToLowerFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}
