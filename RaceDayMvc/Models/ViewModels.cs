using System.ComponentModel.DataAnnotations;

namespace RaceDayMvc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Participant";
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Participant";
    }

    public class EventDto
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = "";
        public decimal DistanceKm { get; set; }
        public string EventType { get; set; } = "";
    }

    public class HomeIndexViewModel
    {
        public List<EventDto> Events { get; set; } = new();
        public List<string> EventTypes { get; set; } = new();
        public string? Search { get; set; }
        public string? Type { get; set; }
        public string? Error { get; set; }
    }
}
