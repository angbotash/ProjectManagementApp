using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;

namespace ProjectManagementApp.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationContext _dbContext;

        public EmployeeRepository(ApplicationContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task Create(Employee newEmployee)
        {
            await this._dbContext.AddAsync(newEmployee);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, string firstName, string lastName, string patronymic, string email)
        {
            var employee = await this._dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.Patronymic = patronymic;
                employee.Email = email;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Employee?> Get(int id)
        {
            var employee = await this._dbContext.Employees.FindAsync(id);

            return employee;
        }

        public async Task<Employee?> Get(string email)
        {
            var employee =await this._dbContext.Employees.FindAsync(email);

            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = this._dbContext.Employees;

            return employees;
        }

        public async Task<IEnumerable<Project>> GetProjects(int id)
        {
            var projects = this._dbContext.EmployeeProject.Where(p => p.EmployeeId == id).ToList();
            var result = new List<Project>();

            foreach (var project in projects)
            {
                var tempProject = await this._dbContext.Projects.FirstOrDefaultAsync(p => p.Id == project.ProjectId);

                if (tempProject != null)
                {
                    result.Add(tempProject);
                }
            }

            return result;
        }

        public async Task<bool> IsEmployeeOnProject(int employeeId, int projectId)
        {
            var employeeProject =
                await this._dbContext.EmployeeProject.FirstOrDefaultAsync(x =>
                    x.EmployeeId == employeeId && x.ProjectId == projectId);

            if (employeeProject is null)
            {
                return false;
            }

            return true;
        }

        public async Task Delete(int id)
        {
            var employee = await this._dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                this._dbContext.Employees.Remove(employee);
                this._dbContext.SaveChanges();
            }
        }
    }
}
