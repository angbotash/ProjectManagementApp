﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        void Create(Project newProject);

        Task Update(int id, string name, string clientCompanyName, string executorCompanyName, DateTime startDate, DateTime endDate, int priority);

        Project? GetById(int id);

        void Delete(int id);
    }
}
