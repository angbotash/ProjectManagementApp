using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Web.ViewModels
{
    public class CreateProjectViewModel
    {
        [Required(ErrorMessage = "Project name is required.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Client company name is required.")]
        [DataType(DataType.Text)]
        public string ClientCompanyName { get; set; }

        [Required(ErrorMessage = "Executor company name is required.")]
        [DataType(DataType.Text)]
        public string ExecutorCompanyName { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Priority 1-10 is required.")]
        public int Priority { get; set; }
    }
}
