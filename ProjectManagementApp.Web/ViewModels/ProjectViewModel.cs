using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientCompanyName { get; set; }
        public string ExecutorCompanyName { get; set; }
        public EmployeeViewModel? Manager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public IList<EmployeeViewModel> Employees { get; set; }
    }
}
