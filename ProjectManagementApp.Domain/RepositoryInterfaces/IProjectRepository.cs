﻿using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task CreateAsync(Project newProject);

        Task UpdateAsync(Project updatedProject);

        Task DeleteAsync(int id);

        Task<Project?> GetByIdAsync(int id);

        Task<IList<Project>> GetManagerProjectsAsync(int managerId);

        Task<IList<Project>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);
    }
}
