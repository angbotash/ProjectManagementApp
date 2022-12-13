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
            await this._dbContext.Employees.AddAsync(newEmployee);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Update(Employee updatedEmployee)
        {
            var employee = this._dbContext.Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if (employee != null)
            {
                employee.FirstName = updatedEmployee.FirstName;
                employee.LastName = updatedEmployee.LastName;
                employee.Patronymic = updatedEmployee.Patronymic;
                employee.Email = updatedEmployee.Email;

                await _dbContext.SaveChangesAsync();
            }
        }

        public Employee? Get(int id)
        {
            var employee = this._dbContext.Employees
                .Include(x => x.AssignedIssues)
                    .ThenInclude(x => x.Reporter)
                .Include(x => x.AssignedIssues)
                    .ThenInclude(x => x.Project)
                .Include(x => x.ReportedIssues)
                    .ThenInclude(x => x.Assignee)
                .Include(x => x.ReportedIssues)
                    .ThenInclude(x => x.Project)
                .FirstOrDefault(e => e.Id == id);

            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = this._dbContext.Employees;

            return employees;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = this._dbContext.EmployeeProject
                .Where(p => p.EmployeeId == id)
                .ToList();
            var result = new List<Project>();

            foreach (var project in projects)
            {
                var tempProject = this._dbContext.Projects.FirstOrDefault(p => p.Id == project.ProjectId);

                if (tempProject != null)
                {
                    result.Add(tempProject);
                }
            }

            return result;
        }

        public async Task AddToProject(int projectId, int employeeId)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject =
                this._dbContext.EmployeeProject.FirstOrDefault(p =>
                    p.ProjectId == projectId && p.EmployeeId == employeeId);

            if (employeeProject is null)
            {
                var newEmployeeProject = new EmployeeProject()
                    { EmployeeId = employeeId, ProjectId = projectId };

                await this._dbContext.EmployeeProject.AddAsync(newEmployeeProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFromProject(int projectId, int employeeId)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject =
                this._dbContext.EmployeeProject.FirstOrDefault(p =>
                    p.ProjectId == projectId && p.EmployeeId == employeeId);

            if (employeeProject != null)
            {
                this._dbContext.EmployeeProject.Remove(employeeProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public bool IsEmployeeOnProject(int employeeId, int projectId)
        {
            var employeeProject =
                this._dbContext.EmployeeProject.FirstOrDefault(x =>
                    x.EmployeeId == employeeId && x.ProjectId == projectId);

            if (employeeProject is null)
            {
                return false;
            }

            return true;
        }

        public async Task Delete(int id)
        {
            var employee = this._dbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                this._dbContext.Employees.Remove(employee);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
