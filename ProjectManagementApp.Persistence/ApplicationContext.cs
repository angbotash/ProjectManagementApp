using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Persistence.Configurations;

namespace ProjectManagementApp.Persistence
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<EmployeeProject> EmployeeProject { get; set; } = null!;
        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMappingConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeProjectMappingConfiguration());
        }
    }
}