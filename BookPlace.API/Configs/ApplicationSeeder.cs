using BookPlace.Core.Domain.Entities;
using BookPlace.Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace BookPlace.API.Configs
{
    public class ApplicationSeeder
    {
        public static async Task SeedDefaultUserRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { UserRole.SuperAdmin.ToString(), UserRole.Admin.ToString(), UserRole.Employee.ToString(), UserRole.User.ToString() };
            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedDefaultSuperAdminUser(UserManager<User> userManager)
        {
            var superAdminUser = await userManager.FindByNameAsync("superadmin");
            if (superAdminUser == null) {
                superAdminUser = new User
                {
                    UserName = "superadmin",
                    Name = "Fisnik",
                    Surname = "Alidemi",
                    Email = "fisnik_alidemi@hotmail.com",
                    NormalizedEmail = "FISNIK_ALIDEMI@HOTMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    IsDeleted = false,
                    CreatedOnDate = DateTime.UtcNow,
                    ModifiedOnDate = DateTime.UtcNow
                };

                // Create the user
                await userManager.CreateAsync(superAdminUser, "SuperAdmin123!");
                // Assign the user to the SuperAdmin role
                await userManager.AddToRoleAsync(superAdminUser, UserRole.SuperAdmin.ToString());
            }
        }
    }
}
