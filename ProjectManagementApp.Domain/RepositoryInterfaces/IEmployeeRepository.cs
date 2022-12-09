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
        void Create(Employee newEmployee);

        Task Update(int id, string firstName, string lastName, string patronymics, string email);

        Employee? GetById(int id);

        void Delete(int id);
    }
}
