using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        public void Create(Project newProject);

        public void Update(Project updatedProject);

        public Project GetById(int id);

        public void Delete(int id);
    }
}
