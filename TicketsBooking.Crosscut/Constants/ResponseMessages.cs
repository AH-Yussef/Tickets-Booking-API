namespace TicketsBooking.Crosscut.Constants
{
    public static class ResponseMessages
    {
        public static string Success => "your operation has been completed successfully";
        public static string NotFound => "your query has no result";
        public static string Failure => " your operation doesn't completed successfully, please check your input and try again";
        public static string Unauthorized => "sorry, you are not authorized to perform this action";
        public static string Unauthenticated => "login failed. wrong user credentials";
        public static string UnprocessableEntity => "Input values are not valid";
    }
}
