using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class UserProjectMappingConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.HasKey(up => new { UserId = up.UserId, up.ProjectId });

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserProject)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(up => up.Project)
                .WithMany(p => p.UserProject)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
