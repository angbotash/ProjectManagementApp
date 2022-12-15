using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(Project updatedProject);

        Task Delete(int id);

        Task<Project?> GetById(int id);

        Task<IList<Project>> GetAll();

        Task<IList<Project>> GetManagerProjects(int managerId);
    }
}
