namespace ProjectManagementApp.Web.ViewModels
{
    public class EditProjectEmployeesViewModel
    {
        public ProjectViewModel Project { get; set; } = null!;

        public IList<UserViewModel> ProjectEmployees { get; set; } = new List<UserViewModel>();

        public IList<UserViewModel> AllEmployees { get; set; } = new List<UserViewModel>();
    }
}
