
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }

    }
}
 