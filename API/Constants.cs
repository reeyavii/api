namespace API
{
    public class Constants
    {
        public static string DateTimeFormat = "dddd, dd MMMM yyyy hh:mm:ss tt";
        public static List<string> paymentStatus = new List<string>(){
            "Pending",
            "Sent through Gcash",
            "Pending Confirmation",
            "Payment Success"
            };
    }
}
