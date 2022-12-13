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
            await this._dbContext.Projects.AddAsync(newProject);
            await this._dbContext.SaveChangesAsync();

            if (newProject.ManagerId != null)
            {
                var project = this._dbContext.Projects.FirstOrDefault(x =>
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

        public async Task Update(Project updatedProject)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == updatedProject.Id);

            if (project != null)
            {
                project.Name = updatedProject.Name;
                project.ClientCompanyName = updatedProject.ClientCompanyName;
                project.ExecutorCompanyName = updatedProject.ExecutorCompanyName;
                project.StartDate = updatedProject.StartDate;
                project.EndDate = updatedProject.EndDate;
                project.Priority = updatedProject.Priority;
                project.ManagerId = updatedProject.ManagerId;

                await this._dbContext.SaveChangesAsync();
            }
        }

        public Project? Get(int id)
        {
            var project = this._dbContext.Projects.Include(x => x.Manager)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Assignee)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Reporter)
                .FirstOrDefault(p => p.Id == id);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = this._dbContext.Projects
                .Include(x => x.Manager)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Assignee)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Reporter);

            return projects;
        }

        public IEnumerable<Employee> GetEmployees(int id)
        {
            var employees = this._dbContext.EmployeeProject.Where(e => e.ProjectId == id).ToList();
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

        public async Task Delete(int id)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == id);

            if (project != null)
            {
                this._dbContext.Projects.Remove(project);
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
    }
}
