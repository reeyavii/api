namespace API.Models
{
    public class LesseeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; } = "requested";
        public int  StallNumber { get; set; }
        public string  StallType { get; set; }
        public string CivilStatus { get; set; }
        public string Brgy { get; set; }
        public string Municipality { get; set; }
        public string ZipCode { get; set; }
        public string Province { get; set; }
        public string UserId { get; set; }

    }
}
