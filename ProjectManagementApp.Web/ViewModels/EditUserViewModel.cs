using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string? Patronymic { get; set; } 

        [Required(ErrorMessage = "Invalid email.")]
        [DataType((DataType.EmailAddress))]
        public string Email { get; set; } = null!;
    }
}
