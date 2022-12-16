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
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<ProjectViewModel, Project>().ReverseMap();
            CreateMap<EditUserViewModel, User>().ReverseMap();
            CreateMap<EditProjectViewModel, Project>().ReverseMap();
            CreateMap<CreateIssueViewModel, Issue>();
            CreateMap<IssueViewModel, Issue>().ReverseMap();
            CreateMap<Issue, EditIssueViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, User>();
        }
    }
}
