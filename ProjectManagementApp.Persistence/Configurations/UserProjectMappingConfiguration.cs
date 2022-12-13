using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class UserProjectMappingConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.HasKey(k => new { UserId = k.UserId, k.ProjectId });

            builder.HasOne(ep => ep.User)
                .WithMany(e => e.UserProject)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ep => ep.Project)
                .WithMany(p => p.UserProject)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
