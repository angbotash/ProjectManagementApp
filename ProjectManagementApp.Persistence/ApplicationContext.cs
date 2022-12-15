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

            SeedRoles(modelBuilder);
            SeedSupervisor(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "e10dcc3f-a309-441f-9752-a6814f6162cf"
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "fd21c7cd-8658-45cf-bd8c-d4ffac3c0ae5"
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "fde66fdb-68cf-444e-9b69-f12141c880f8"
                });
        }

        private static void SeedSupervisor(ModelBuilder modelBuilder)
        {
            // Add supervisor
            var supervisor = new User()
            {
                Id = 1,
                FirstName = "Super",
                LastName = "Visor",
                Email = "supervisor@email.com",
                NormalizedEmail = "SUPERVISOR@EMAIL.COM",
                UserName = "supervisor@email.com",
                NormalizedUserName = "SUPERVISOR@EMAIL.COM",
                SecurityStamp = "LSXZ6IVP5EFL69WLQPVRJRJ2RM45HT2T",
                LockoutEnabled = true,
            };

            // Set password
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            supervisor.PasswordHash = passwordHasher.HashPassword(supervisor, "jsnf36gxdo");

            // Seed supervisor
            modelBuilder.Entity<User>().HasData(supervisor);

            // Seed supervisor's role
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.HasData(new
                {
                    RoleId = 1,
                    UserId = 1
                });
            });
        }
    }
}