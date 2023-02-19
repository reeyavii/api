#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class UserInformation
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public int? StallId {get; set; }
        public Stall? Stall { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleInitial { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; } = "requested";
        public string? delinquent { get; set; } = "no";
        public string? Created { get; set; } = DateTime.Now.ToString(Constants.DateTimeFormat);
        public string? ApprovedDate { get; set; }
        public string? CivilStatus { get; set; }
        public string? EditAllowed { get; set; } = "no";
        public string? EditRequested {get; set; } = "no"; // if yes notice already sent: if no will display send notice
        public string? Municipality { get; set; }
        public string? Province { get; set; }
        public string? ZipCode { get; set; }
        public string? Brgy { get; set; }









    }
}
