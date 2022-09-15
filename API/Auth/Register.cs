using System.ComponentModel.DataAnnotations;

namespace API.Auth
{
    public class Register
    {
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Middle Initial is required")]
        public string? MiddleInitial { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public string? Sex { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
