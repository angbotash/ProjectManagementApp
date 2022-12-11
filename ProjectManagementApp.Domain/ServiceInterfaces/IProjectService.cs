using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        void Create(Project newProject);

        Task Edit(Project updatedProject);

        void Delete(int id);
    }
}
