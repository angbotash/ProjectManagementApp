using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class EmployeeProjectMappingConfiguration : IEntityTypeConfiguration<EmployeeProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeProject> builder)
        {
            builder.HasKey(k => new { k.EmployeeId, k.ProjectId });

            builder.HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProject)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProject)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
