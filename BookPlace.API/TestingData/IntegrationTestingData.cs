using BookPlace.Core.Domain.Entities;
using BookPlace.Core.Domain.Enum;
using BookPlace.Infrastructure.Data;

namespace BookPlace.API.TestingData
{
    public static class IntegrationTestingData
    {
        public static void AddDefaultUserRoles(AppDbContext db)
        {
            db.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "33a0c1ef-b157-4dba-b61a-865913926195", Name = UserRole.SuperAdmin.ToString() });
            db.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "cbda618a-19de-4be3-9764-53d248de5f16", Name = UserRole.Admin.ToString() });
            db.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "a6dde321-a61d-449e-9b9a-014ab9e51585", Name = UserRole.Employee.ToString() });
            db.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "3d0e9b3e-b826-4d08-a6ec-769b3f0ef472", Name = UserRole.User.ToString() });
        }

        public static void AddDefaultUsersAndTheirRoles(AppDbContext db)
        {
            db.Users.Add(new User
            {
                Id = "8e07c860-8398-4739-a7f8-c53c786a14ce",
                UserName = "superadmin",
                Name = "Fisnik",
                Surname = "Alidemi",
                Email = "fisnik_alidemi@hotmail.com",
                NormalizedEmail = "FISNIK_ALIDEMI@HOTMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = "3b564104-e3f2-4c6d-b660-03cf6934089c",
                ConcurrencyStamp = "dbf4bfc7-d775-4e00-939e-4e4f6f3b5604",
                PhoneNumber = "",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                IsDeleted = false,
                CreatedOnDate = DateTime.UtcNow,
                ModifiedOnDate = DateTime.UtcNow
            });
            db.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
            {
                UserId = "8e07c860-8398-4739-a7f8-c53c786a14ce",
                RoleId = "33a0c1ef-b157-4dba-b61a-865913926195"
            });
        }

        public static void AddDefaultReservations(AppDbContext db)
        {
            db.Reservations.Add(new Reservation
            {
                Id = Guid.Parse("17a8e4ba-29fd-414e-9302-6a62b55a28af"),
                Email = "laidan.broci@mailinator.com",
                IsConfirmed = true,
                IsDeleted = false,
                NameAndSurname = "Laidan Broci",
                ReservationFrom = new DateTime(2025, 4, 29, 12, 0, 0),
                ReservationTo = new DateTime(2025, 4, 29, 13, 0, 0),
                CreateByUserId = Guid.Parse("8e07c860-8398-4739-a7f8-c53c786a14ce"),
                CreatedOnDate = DateTime.UtcNow,
                ModifiedOnDate = DateTime.UtcNow,
            });
            db.Reservations.Add(new Reservation
            {
                Id = Guid.Parse("47e3921e-6c37-4b34-9e13-148f140deafd"),
                Email = "flladim.pula@mailinator.com",
                IsConfirmed = false,
                IsDeleted = false,
                NameAndSurname = "Flladim Pula",
                ReservationFrom = new DateTime(2025, 5, 10, 12, 0, 0),
                ReservationTo = new DateTime(2025, 5, 10, 13, 0, 0),
                CreateByUserId = Guid.Parse("8e07c860-8398-4739-a7f8-c53c786a14ce"),
                CreatedOnDate = DateTime.UtcNow,
                ModifiedOnDate = DateTime.UtcNow,
            });
            db.Reservations.Add(new Reservation
            {
                Id = Guid.Parse("986c0ee0-a114-4778-a1c3-845879952432"),
                Email = "besfort.hysi@mailinator.com",
                IsConfirmed = false,
                IsDeleted = false,
                NameAndSurname = "Besfort Hysi",
                ReservationFrom = new DateTime(2025, 5, 1, 12, 0, 0),
                ReservationTo = new DateTime(2025, 5, 1, 13, 0, 0),
                CreateByUserId = Guid.Parse("8e07c860-8398-4739-a7f8-c53c786a14ce"),
                CreatedOnDate = DateTime.UtcNow,
                ModifiedOnDate = DateTime.UtcNow,
            });
        }
    }
}
