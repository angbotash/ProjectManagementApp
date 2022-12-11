using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class ProjectMappingConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            builder.HasMany(p => p.EmployeeProject)
                   .WithOne(ep => ep.Project)
                   .HasForeignKey(p => p.ProjectId);

            builder.HasOne(p => p.TeamLeader);
        }
    }
}
