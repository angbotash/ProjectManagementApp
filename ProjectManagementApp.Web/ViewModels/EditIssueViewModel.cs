using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementApp.Web.ViewModels
{
    public class EditIssueViewModel
    { 
        public int Id { get; set; }

        [Required(ErrorMessage = "Issue name is required.")]
        [StringLength(100, MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Issue name")]
        public string Name { get; set; } = null!;

        public int ReporterId { get; set; }

        [Required(ErrorMessage = "Assignee is required.")]
        [Display(Name = "Assignee")]
        public int AssigneeId { get; set; }

        public UserViewModel? Assignee { get; set; }

        public int ProjectId { get; set; }

        [DataType(DataType.Text)]
        [StringLength(1000)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public IssueStatus Status { get; set; }

        [Required(ErrorMessage = "Priority 1-10 is required.")]
        [Range(1, 10)]
        [Display(Name = "Priority 1-10")]
        public int Priority { get; set; }

        public IList<SelectListItem>? Employees { get; set; }

        public IList<SelectListItem>? Statuses { get; set; }
    }
}
