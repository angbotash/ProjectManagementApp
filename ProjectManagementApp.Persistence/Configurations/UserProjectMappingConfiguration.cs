using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class UserProjectMappingConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.HasKey(up => new { up.UserId, up.ProjectId });

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
