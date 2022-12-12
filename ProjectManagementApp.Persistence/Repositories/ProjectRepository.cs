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

        public async Task Create(Project newProject)
        {
            await this._dbContext.AddAsync(newProject);
            await this._dbContext.SaveChangesAsync();

            if (newProject.ManagerId != null)
            {
                var project = await this._dbContext.Projects.FirstOrDefaultAsync(x =>
                    x.Name == newProject.Name && x.ManagerId == newProject.ManagerId);

                if (project is null)
                {
                    return;
                }

                var newEmployeeProject = new EmployeeProject()
                    {EmployeeId = (int)newProject.ManagerId, ProjectId = project.Id};

                await this._dbContext.EmployeeProject.AddAsync(newEmployeeProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(int id, string name, string clientCompanyName, string executorCompanyName, int? managerId, DateTime startDate, DateTime endDate, int priority)
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

                if (project.ManagerId != null && project.ManagerId != managerId && managerId != null)
                {
                    this.RemoveFromProject(project.Id, (int)project.ManagerId);
                    this.AddToProject(project.Id, (int)managerId);

                }
                project.ManagerId = managerId;

                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task<Project?> Get(int id)
        {
            var project = await this._dbContext.Projects.Include(x => x.Manager).FirstOrDefaultAsync(p => p.Id == id);

            return project;
        }

        public async Task<Project?> Get(string name)
        {
            var project = await this._dbContext.Projects.Include(x => x.Manager).FirstOrDefaultAsync(p => p.Name == name);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = this._dbContext.Projects.Include(x => x.Manager);

            return projects;
        }

        public async Task<IEnumerable<Employee>> GetEmployees(int id)
        {
            var employees = await this._dbContext.EmployeeProject.Where(e => e.ProjectId == id).ToListAsync();
            var result = new List<Employee>();

            foreach (var employee in employees)
            {
                var tempEmployee = await this._dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.EmployeeId);

                if (tempEmployee != null)
                {
                    result.Add(tempEmployee);
                }
            }

            return result;
        }

        public async Task Delete(int id)
        {
            var project = await this._dbContext.Projects.FindAsync(id);

            if (project != null)
            {
                this._dbContext.Projects.Remove(project);
                await this._dbContext.SaveChangesAsync();
            }
        }

        //public void AddToProject(int projectId, IEnumerable<Employee> employees)
        //{
        //    var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

        //    if (project == null)
        //    {
        //        return;
        //    }

        //    foreach (var employee in employees)
        //    {
        //        var employeeProject =
        //            this._dbContext.EmployeeProject.FirstOrDefault(p =>
        //                p.ProjectId == projectId && p.EmployeeId == employee.Id);

        //        if (employeeProject is null)
        //        {
        //            var newEmployeeProject = new EmployeeProject()
        //                { EmployeeId = employee.Id, ProjectId = projectId };

        //            this._dbContext.EmployeeProject.Add(newEmployeeProject);
        //            this._dbContext.SaveChanges();
        //        }
        //    }
        //}

        public async Task AddToProject(int projectId, int employeeId)
        {
            var project = await this._dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject = await 
                this._dbContext.EmployeeProject.FirstOrDefaultAsync(p =>
                    p.ProjectId == projectId && p.EmployeeId == employeeId);

            if (employeeProject is null)
            {
                var newEmployeeProject = new EmployeeProject()
                    { EmployeeId = employeeId, ProjectId = projectId };

                await this._dbContext.EmployeeProject.AddAsync(newEmployeeProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        //public void RemoveFromProject(int projectId, IEnumerable<Employee> employees)
        //{
        //    var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

        //    if (project == null)
        //    {
        //        return;
        //    }

        //    foreach (var employee in employees)
        //    {
        //        var employeeProject =
        //            this._dbContext.EmployeeProject.FirstOrDefault(p =>
        //                p.ProjectId == projectId && p.EmployeeId == employee.Id);

        //        if (employeeProject != null)
        //        {
        //            var tempEmployeeProject = new EmployeeProject()
        //            { EmployeeId = employee.Id, ProjectId = projectId };

        //            this._dbContext.EmployeeProject.Remove(tempEmployeeProject);
        //            this._dbContext.SaveChanges();
        //        }
        //    }
        //}

        public async Task RemoveFromProject(int projectId, int employeeId)
        {
            var project = await this._dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject =
                await this._dbContext.EmployeeProject.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.EmployeeId == employeeId);

            if (employeeProject != null)
            {
                this._dbContext.EmployeeProject.Remove(employeeProject);
                await this._dbContext.SaveChangesAsync();
            }
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
    }
}
