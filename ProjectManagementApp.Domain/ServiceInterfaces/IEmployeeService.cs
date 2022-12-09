using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IEmployeeService
    {
        void Create(Employee newEmployee);

        Task Edit(Employee updatedEmployee);

        Employee? Get(string email);

        void Delete(int id);
    }
}
