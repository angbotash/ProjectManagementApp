using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence.Configurations
{
    public class ProjectMappingConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.UserProjects)
                .WithOne(up => up.Project)
                .HasForeignKey(up => up.ProjectId);

            builder.HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId);

            builder.HasMany(p => p.Issues)
                .WithOne(i => i.Project)
                .HasForeignKey(i => i.ProjectId);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ClientCompanyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ExecutorCompanyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.StartDate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            builder.Property(p => p.Priority)
                .IsRequired();
        }
    }
}
