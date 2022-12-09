using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public void Create(Project newProject)
        {
            var project = this._projectRepository.GetById(newProject.Id);

            if (project == null)
            {
                this._projectRepository.Create(newProject);
            }
        }

        public async Task Edit(Project updatedProject)
        {
            var project = this._projectRepository.GetById(updatedProject.Id);

            if (project != null)
            {
                await this._projectRepository.Update(updatedProject.Id,
                                                     updatedProject.Name,
                                                     updatedProject.ClientCompanyName,
                                                     updatedProject.ExecutorCompanyName,
                                                     updatedProject.StartDate,
                                                     updatedProject.EndDate,
                                                     updatedProject.Priority);
            }
        }

        public void Delete(int id)
        {
            var project = this._projectRepository.GetById(id);

            if (project != null)
            {
                this._projectRepository.Delete(id);
            }
        }
    }
}
