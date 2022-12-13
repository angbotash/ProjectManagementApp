﻿using System.ComponentModel;

namespace ProjectManagementApp.Web.ViewModels.IssueStatus
{
    public enum IssueStatus
    {
        [Description("To Do")]
        ToDo = 0,

        [Description("In Progress")]
        InProgress = 1,

        [Description("Done")]
        Done = 2
    }
}
