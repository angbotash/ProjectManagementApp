using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class EmployeeMappingConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasMany(e => e.EmployeeProject)
                .WithOne(ep => ep.Employee)
                .HasForeignKey(e => e.EmployeeId);

            builder.HasMany(e => e.AssignedIssues)
                .WithOne(t => t.Assignee)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ReportedIssues)
                .WithOne(t => t.Reporter)
                .HasForeignKey(t => t.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
