using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class ProjectMappingConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasMany(p => p.Employees)
                   .WithMany(e => e.Projects);

            builder.HasOne(p => p.TeamLeader)
                .WithMany(t => t.Projects)
                .HasForeignKey(k => k.TeamLeaderId);
        }
    }
}
