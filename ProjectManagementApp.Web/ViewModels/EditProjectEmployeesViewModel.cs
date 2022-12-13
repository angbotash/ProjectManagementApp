namespace ProjectManagementApp.Web.ViewModels
{
    public class EditProjectEmployeesViewModel
    {
        public ProjectViewModel Project { get; set; } = null!;

        public IList<UserViewModel> ProjectUsers { get; set; } = new List<UserViewModel>();

        public IList<UserViewModel> AllUsers { get; set; } = new List<UserViewModel>();
    }
}
