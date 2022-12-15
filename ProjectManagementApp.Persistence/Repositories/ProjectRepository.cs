using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;

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
        }

        public async Task Update(Project updatedProject)
        {
            this._dbContext.Projects.Update(updatedProject);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {id}.");
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project?> GetById(int id)
        {
            return await _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter)
                .Include(p => p.UserProjects)
                    .ThenInclude(up => up.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<Project>> GetAll()
        {
            var projects = _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter);

            return await projects.ToListAsync();
        }

        public async Task<IList<Project>> GetManagerProjects(int managerId)
        {
            var managerProjects = _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter)
                .Where(p => p.ManagerId == managerId);

            return await managerProjects.ToListAsync();
        }
    }
}
