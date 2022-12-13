﻿using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ReporterId { get; set; }

        public EmployeeViewModel Reporter { get; set; } = null!;

        public int AssigneeId { get; set; }

        public EmployeeViewModel Assignee { get; set; } = null!;

        public int ProjectId { get; set; }

        public ProjectViewModel Project { get; set; } = null!;

        public string? Comment { get; set; }

        public IssueStatus.IssueStatus Status { get; set; }

        public int Priority { get; set; }
    }
}
