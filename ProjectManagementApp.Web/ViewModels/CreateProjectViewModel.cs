using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementApp.Web.ViewModels
{
    public class CreateProjectViewModel
    {
        [Required(ErrorMessage = "Project name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Project name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Client company name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Client company name")]

        public string ClientCompanyName { get; set; } = null!;

        [Required(ErrorMessage = "Executor company name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Executor company name")]
        public string ExecutorCompanyName { get; set; } = null!;

        [Display(Name = "Project manager")]
        public int? ManagerId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Priority 1-10 is required.")]
        [Display(Name = "Start date")]
        public int Priority { get; set; }

        public IList<SelectListItem> Users { get; set; } = null!;
    }
}
