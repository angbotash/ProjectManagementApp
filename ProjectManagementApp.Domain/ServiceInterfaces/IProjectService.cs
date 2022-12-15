using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task Create(Project newProject);

        Task Edit(Project updatedProject);

        Task Delete(int id);

        Task<Project?> GetById(int id);

        Task<IEnumerable<Project>> GetAll();

        Task<IEnumerable<Project>> GetManagerProjects(int managerId);
    }
}
