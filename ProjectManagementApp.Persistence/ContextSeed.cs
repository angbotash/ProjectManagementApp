using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Persistence
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole<int>(Role.Supervisor.ToString()));
            await roleManager.CreateAsync(new IdentityRole<int>(Role.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole<int>(Role.Employee.ToString()));
        }
    }
}
