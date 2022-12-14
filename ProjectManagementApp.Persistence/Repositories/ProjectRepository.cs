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
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task Create(Project newProject)
        {
            await _dbContext.Projects.AddAsync(newProject);
            await _dbContext.SaveChangesAsync();

            if (newProject.ManagerId != null)
            {
                var project = _dbContext.Projects
                    .FirstOrDefault(p => p.Name == newProject.Name && p.ManagerId == newProject.ManagerId);

                if (project is null)
                {
                    return;
                }

                var newUserProject = new UserProject
                {
                    UserId = (int)newProject.ManagerId,
                    ProjectId = project.Id
                };

                await _dbContext.UserProject.AddAsync(newUserProject);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Project updatedProject)
        {
            //this._dbContext.Projects.Update(updatedProject);

            //await this._dbContext.SaveChangesAsync();

            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == updatedProject.Id);

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

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);

            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Project? Get(int id)
        {
            var project = _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter)
                .FirstOrDefault(p => p.Id == id);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter);

            return projects;
        }

        public IEnumerable<Project> GetManagerProjects(int managerId)
        {
            var managerProjects = _dbContext.Projects
                .Where(p => p.ManagerId == managerId)
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter);

            return managerProjects;
        }

        public IEnumerable<User> GetUsers(int id)
        {
            var users = _dbContext.UserProject.Where(up => up.ProjectId == id).ToList();
            var result = new List<User>();

            foreach (var user in users)
            {
                var tempUser = _userManager.Users.FirstOrDefault(u => u.Id == user.UserId);

                if (tempUser != null)
                {
                    result.Add(tempUser);
                }
            }

            return result;
        }

        public bool IsUserOnProject(int userId, int projectId)
        {
            var userProject = _dbContext.UserProject.FirstOrDefault(up => up.UserId == userId && up.ProjectId == projectId);

            return userProject != null;
        }
    }
}
