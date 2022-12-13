using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementApp.Web.ViewModels
{
    public class CreateIssueViewModel
    {
        [Required(ErrorMessage = "Issue name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Issue name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Reporter is required.")]
        [Display(Name = "Reporter")]
        public int ReporterId { get; set; }

        [Required(ErrorMessage = "Assignee is required.")]
        [Display(Name = "Assignee")]
        public int AssigneeId { get; set; }

        public int ProjectId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Project status")]
        public IssueStatus.IssueStatus Status { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [Display(Name = "Priority")]
        public int Priority { get; set; }

        public IList<SelectListItem> Users { get; set; } = null!;

        public IList<SelectListItem> Statuses { get; set; } = null!;
    }
}
