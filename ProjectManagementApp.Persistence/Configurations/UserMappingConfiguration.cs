using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class UserMappingConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(e => e.UserProject)
                .WithOne(ep => ep.User)
                .HasForeignKey(e => e.UserId);

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
