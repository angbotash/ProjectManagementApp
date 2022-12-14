using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;

namespace ProjectManagementApp.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ProjectRepository(ApplicationContext dbContext, UserManager<User> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        public async Task Create(Project newProject)
        {
            await this._dbContext.Projects.AddAsync(newProject);
            await this._dbContext.SaveChangesAsync();

            if (newProject.ManagerId != null)
            {
                var project = this._dbContext.Projects
                    .FirstOrDefault(x => x.Name == newProject.Name && x.ManagerId == newProject.ManagerId);

                if (project is null)
                {
                    return;
                }

                var newUserProject = new UserProject
                {
                    UserId = (int)newProject.ManagerId,
                    ProjectId = project.Id
                };

                await this._dbContext.UserProject.AddAsync(newUserProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Project updatedProject)
        {
            //this._dbContext.Projects.Update(updatedProject);

            //await this._dbContext.SaveChangesAsync();

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
            }

            await this._dbContext.SaveChangesAsync();
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

        public Project? Get(int id)
        {
            var project = this._dbContext.Projects
                .Include(x => x.Manager)
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

        public IEnumerable<Project> GetManagerProjects(int managerId)
        {
            var managerProjects = this._dbContext.Projects
                .Where(x => x.ManagerId == managerId)
                .Include(x => x.Manager)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Assignee)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.Reporter);

            return managerProjects;
        }

        public IEnumerable<User> GetUsers(int id)
        {
            var users = this._dbContext.UserProject.Where(e => e.ProjectId == id).ToList();
            var result = new List<User>();

            foreach (var user in users)
            {
                var tempUser = this._userManager.Users.FirstOrDefault(e => e.Id == user.UserId);

                if (tempUser != null)
                {
                    result.Add(tempUser);
                }
            }

            return result;
        }

        public bool IsUserOnProject(int userId, int projectId)
        {
            var userProject = this._dbContext.UserProject
                .FirstOrDefault(x => x.UserId == userId && x.ProjectId == projectId);

            return userProject != null;
        }
    }
}
