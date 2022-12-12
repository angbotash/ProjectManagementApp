using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Text)]
        public string? Patronymic { get; set; } 

        [Required(ErrorMessage = "Invalid email.")]
        [DataType((DataType.EmailAddress))]
        public string Email { get; set; } = null!;
    }
}
