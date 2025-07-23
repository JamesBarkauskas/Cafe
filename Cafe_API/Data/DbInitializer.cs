using Cafe_API.Models;
using Microsoft.AspNetCore.Identity;

namespace Cafe_API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // create the two roles
            string[] roles = { "Admin", "Customer" };

            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // seed Admin user
            string userName = "James123";
            string password = "Test123*";   // must meet Identity password policy.. (upper, num, char, >7)

            var adminUser = await userManager.FindByNameAsync(userName);
            if (adminUser == null)
            {
                // create that adminUser..
                adminUser = new AppUser
                {
                    UserName = userName,
                    Name = "James",
                    Email = userName + "@gmail.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // log exception..
                    throw new Exception("Failed to create default admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
