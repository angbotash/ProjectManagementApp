using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class ProjectMappingConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            builder.HasMany(p => p.UserProject)
                .WithOne(ep => ep.Project)
                .HasForeignKey(p => p.ProjectId);

            builder.HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId);

            builder.HasMany(p => p.Issues)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);
        }
    }
}
