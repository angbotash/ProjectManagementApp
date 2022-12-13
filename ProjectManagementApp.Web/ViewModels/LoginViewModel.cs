using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Invalid email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Invalid password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
