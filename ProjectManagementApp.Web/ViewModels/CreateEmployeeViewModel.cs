using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Text)]
        [Display(Name = "Patronymic")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Invalid email.")]
        [DataType((DataType.EmailAddress))]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
    }
}
