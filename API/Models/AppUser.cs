using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleInitial { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? Status { get; set; }
        public string? Address { get; set; }
        public string? EmployeeId { get; set; }
        public bool? Verified { get; set; }
        //public Payment Payment { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image {get; set; }


    }
}
