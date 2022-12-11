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
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProjectRepository(ApplicationContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Create(Project newProject)
        {
            this._dbContext.Add(newProject);
            this._dbContext.SaveChanges();
        }

        public async Task Update(int id, string name, string clientCompanyName, string executorCompanyName, DateTime startDate, DateTime endDate, int priority)
        {
            var project = await this._dbContext.Projects.FindAsync(id);

            if (project != null)
            {
                project.Name = name;
                project.ClientCompanyName = clientCompanyName;
                project.ExecutorCompanyName = executorCompanyName;
                project.StartDate = startDate;
                project.EndDate = endDate;
                project.Priority = priority;

                await this._dbContext.SaveChangesAsync();
            }
        }

        public Project? GetById(int id)
        {
            var project = this._dbContext.Projects.Include(x => x.Manager).FirstOrDefault(p => p.Id == id);

            return project;
        }

        public Project? GetByName(string name)
        {
            var project = this._dbContext.Projects.Include(x => x.Manager).FirstOrDefault(p => p.Name == name);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = this._dbContext.Projects.Include(x => x.Manager);

            return projects;
        }

        public IEnumerable<Employee> GetEmployees(int id)
        {
            var employees = this._dbContext.EmployeeProject.Where(e => e.ProjectId == id);
            var result = new List<Employee>();

            foreach (var employee in employees)
            {
                var tempEmployee = this._dbContext.Employees.FirstOrDefault(e => e.Id == employee.EmployeeId);

                if (tempEmployee != null)
                {
                    result.Add(tempEmployee);
                }
            }

            return result;
        }

        public void Delete(int id)
        {
            var project = this._dbContext.Projects.Find(id);

            if (project != null)
            {
                this._dbContext.Projects.Remove(project);
                this._dbContext.SaveChanges();
            }
        }
    }
}
