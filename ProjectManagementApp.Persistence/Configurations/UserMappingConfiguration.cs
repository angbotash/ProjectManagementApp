using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

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

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.Patronymic)
                .HasMaxLength(30);
        }
    }
}
