using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Patronymic")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Invalid email.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Invalid password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = null!;

        public string? Role { get; set; }
    }
}
