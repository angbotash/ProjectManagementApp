﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Create(Employee newEmployee)
        {
            this._dbContext.Add(newEmployee);
            this._dbContext.SaveChanges();
        }

        public async Task Update(int id, string firstName, string lastName, string patronymic, string email)
        {
            var employee = await this._dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.Patronymic = patronymic;
                employee.Email = email;

                await _dbContext.SaveChangesAsync();
            }
        }

        public Employee? GetById(int id)
        {
            var employee = this._dbContext.Employees.Find(id);

            return employee;
        }

        public Employee? GetByEmail(string email)
        {
            var employee = this._dbContext.Employees.Find(email);

            return employee;
        }

        public void Delete(int id)
        {
            var employee = _dbContext.Employees.Find(id);

            if (employee != null)
            {
                this._dbContext.Employees.Remove(employee);
                this._dbContext.SaveChanges();
            }
        }
    }
}