namespace ProjectManagementApp.Web.ViewModels
{
    public class EditProjectEmployeesViewModel
    {
        public ProjectViewModel Project { get; set; } = null!;

        public IList<EmployeeViewModel> ProjectEmployees { get; set; } = new List<EmployeeViewModel>();

        public IList<EmployeeViewModel> AllEmployees { get; set; } = new List<EmployeeViewModel>();
    }
}
