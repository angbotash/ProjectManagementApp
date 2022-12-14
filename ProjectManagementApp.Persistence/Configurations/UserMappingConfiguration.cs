using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class UserMappingConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.UserProject)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId);

            builder.HasMany(u => u.AssignedIssues)
                .WithOne(i => i.Assignee)
                .HasForeignKey(i => i.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReportedIssues)
                .WithOne(i => i.Reporter)
                .HasForeignKey(i => i.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
