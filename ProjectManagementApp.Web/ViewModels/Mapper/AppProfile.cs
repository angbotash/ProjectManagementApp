using AutoMapper;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.ViewModels.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CreateUserViewModel, User>();
            CreateMap<CreateProjectViewModel, Project>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<ProjectViewModel, Project>();
            CreateMap<Project, ProjectViewModel>();
            CreateMap<EditUserViewModel, User>();
            CreateMap<User, EditUserViewModel>();
            CreateMap<EditProjectViewModel, Project>();
            CreateMap<Project, EditProjectViewModel>();
            CreateMap<CreateIssueViewModel, Issue>();
            CreateMap<IssueViewModel, Issue>();
            CreateMap<Issue, IssueViewModel>();
            CreateMap<Issue, EditIssueViewModel>();
            CreateMap<EditIssueViewModel, Issue>();
            CreateMap<RegisterViewModel, User>();
        }
    }
}
