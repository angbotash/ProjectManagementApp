﻿using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task Create(Employee newEmployee);

        Task Edit(Employee updatedEmployee);

        Employee? Get(int id);

        IEnumerable<Employee> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task AddToProject(int projectId, int employeeId);

        Task RemoveFromProject(int projectId, int employeeId);

        Task Delete(int id);
    }
}
