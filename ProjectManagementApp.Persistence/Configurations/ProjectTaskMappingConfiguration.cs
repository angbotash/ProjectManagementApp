using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class ProjectTaskMappingConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasOne(t => t.Reporter).WithMany().HasForeignKey(t => t.ReporterId);
            builder.HasOne(t => t.Project).WithMany().HasForeignKey(t => t.ProjectId);
        }
    }
}
