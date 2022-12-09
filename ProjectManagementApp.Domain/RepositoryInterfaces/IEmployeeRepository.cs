using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        public void Create(Employee newEmployee);
        public void Update(Employee updatedEmployee);
        public Employee GetById(int id);
        public void Delete(int id);
    }
}
