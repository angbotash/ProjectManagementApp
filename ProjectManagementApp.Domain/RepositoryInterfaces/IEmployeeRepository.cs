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

        Task Update(int id, string firstName, string lastName, string patronymic, string email);

        Employee? GetById(int id);

        Employee? GetByEmail(string email);

        void Delete(int id);
    }
}
