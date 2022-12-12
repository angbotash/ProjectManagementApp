namespace ProjectManagementApp.Web.ViewModels
{
    public class EditProjectEmployeesViewModel
    {
        public ProjectViewModel Project { get; set; }
        public IList<EmployeeViewModel> ProjectEmployees { get; set; } = new List<EmployeeViewModel>();
        public IList<EmployeeViewModel> AllEmployees { get; set; } = new List<EmployeeViewModel>();
    }
}
