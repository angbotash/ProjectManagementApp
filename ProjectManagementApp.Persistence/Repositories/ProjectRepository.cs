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
                var project = this._dbContext.Projects.FirstOrDefault(x => x.Name == newProject.Name && x.ManagerId == newProject.ManagerId);

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

                if (project.ManagerId != null && project.ManagerId != updatedProject.ManagerId && updatedProject.ManagerId != null)
                {
                    await this.RemoveFromProject(project.Id, (int)project.ManagerId);
                    await this.AddToProject(project.Id, (int)updatedProject.ManagerId);

                }
                project.ManagerId = updatedProject.ManagerId;

                await this._dbContext.SaveChangesAsync();
            }
        }

        public Project? Get(int id)
        {
            var project = this._dbContext.Projects.Include(x => x.Manager).FirstOrDefault(p => p.Id == id);

            return project;
        }

        public Project? Get(string name)
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
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject = this._dbContext.EmployeeProject.FirstOrDefault(p => p.ProjectId == projectId && p.EmployeeId == employeeId);

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
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var employeeProject =
                this._dbContext.EmployeeProject.FirstOrDefault(p => p.ProjectId == projectId && p.EmployeeId == employeeId);

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
    }
}
