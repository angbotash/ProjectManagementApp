using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Persistence.Configurations;

namespace ProjectManagementApp.Persistence
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<UserProject> UserProject { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMappingConfiguration());
            modelBuilder.ApplyConfiguration(new UserProjectMappingConfiguration());
            modelBuilder.ApplyConfiguration(new IssueMappingConfiguration());
        }
    }
}