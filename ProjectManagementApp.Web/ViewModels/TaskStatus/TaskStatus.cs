using System.ComponentModel;

namespace ProjectManagementApp.Web.ViewModels.TaskStatus
{
    public enum TaskStatus
    {
        [Description("To Do")]
        ToDo,

        [Description("In Progress")]
        InProgress,

        [Description("Done")]
        Done
    }
}
