using AutoMapper;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.ViewModels.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CreateEmployeeViewModel, Employee>();
            CreateMap<CreateProjectViewModel, Project>();
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<ProjectViewModel, Project>();
            CreateMap<Project, ProjectViewModel>();
            CreateMap<EditEmployeeViewModel, Employee>();
            CreateMap<Employee, EditEmployeeViewModel>();
            CreateMap<EditProjectViewModel, Project>();
            CreateMap<Project, EditProjectViewModel>();
            CreateMap<CreateIssueViewModel, Issue>();
            CreateMap<IssueViewModel, Issue>();
            CreateMap<Issue, IssueViewModel>();
            CreateMap<Issue, EditIssueViewModel>();
            CreateMap<EditIssueViewModel, Issue>();
        }
    }
}
