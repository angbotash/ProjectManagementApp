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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        public void Create(Employee newEmployee)
        {
            var employee = this._employeeRepository.GetById(newEmployee.Id);

            if (employee == null)
            {
                this._employeeRepository.Create(newEmployee);
            }
        }

        public async Task Edit(Employee updatedEmployee)
        {
            var employee = this._employeeRepository.GetById(updatedEmployee.Id);

            if (employee != null)
            {
                await this._employeeRepository.Update(updatedEmployee.Id,
                                                      updatedEmployee.FirstName,
                                                      updatedEmployee.LastName, 
                                                      updatedEmployee.Patronymic,
                                                      updatedEmployee.Email);
            }
        }

        public void Delete(int id)
        {
            var employee = this._employeeRepository.GetById(id);

            if (employee != null)
            {
                this._employeeRepository.Delete(id);
            }
        }
    }
}
