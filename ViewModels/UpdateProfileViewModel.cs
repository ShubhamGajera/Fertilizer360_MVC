using System.ComponentModel.DataAnnotations;

namespace Fertilizer360.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }  // User ID (hidden in form)

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool ChangePassword { get; set; } // Checkbox to toggle password fields

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }
    }
}
