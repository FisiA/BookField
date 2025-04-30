using BookPlace.API.TestingData;
using BookPlace.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookPlace.IntegrationTests.Utils.Users
{
    public class UsersCustomFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set the enviroment to Testing manually in order to use InMemoryDb
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DBContext registration for integration tests
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new in-memory DBContext for testing purposes
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("UsersControllerTestIMDb");
                });

                // Run seeders here
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    try
                    {
                        SeedDatabase(db);
                    }
                    catch (Exception exc)
                    {
                        throw;
                    }
                }
            });
        }

        private void SeedDatabase(AppDbContext db)
        {
            // Add default user roles
            IntegrationTestingData.AddDefaultUserRoles(db);

            // Add default users and their roles
            IntegrationTestingData.AddDefaultUsersAndTheirRoles(db);

            db.SaveChanges();
        }
    }
}
