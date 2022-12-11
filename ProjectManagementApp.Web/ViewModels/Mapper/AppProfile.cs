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
        }
    }
}
