using System.ComponentModel.DataAnnotations;

namespace API.Auth
{
    public class RegisterAdmin
    {
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Store Name is required")]
        public string? EmployeeId { get; set; }
        public string? MiddleInitial { get; set; }
        public string? Address { get; set; }
      
    }
}
