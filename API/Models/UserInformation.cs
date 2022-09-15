#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class UserInformation
    {
        public int Id { get; set; }
        public int StallId {get; set; }
        public Stall Stall { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; } = "requested";
        public string Created { get; set; } = DateTime.Now.ToString(Constants.DateTimeFormat);
       
       
       
       
       
       
    }
}
