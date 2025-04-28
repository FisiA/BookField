using BookPlace.API.TestingData;
using BookPlace.Core.Domain.Entities;
using BookPlace.Core.Domain.Enum;
using BookPlace.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookPlace.IntegrationTests.Utils
{
    // WebApplicationFactory spins up a real mini web server in memory — without needing to actually deploy or run the app manually
    // With that in place you get: real HTTP pipeline, real middleware (auth, exception handling, routing etc.) and real DI (services, repositories)
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set the enviroment to Testing manually in order to use InMemoryDb
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestingScheme";
                    options.DefaultChallengeScheme = "TestingScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestingScheme", options => { });
                services.AddAuthorization();

                // Remove existing DBContext registration for integration tests
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if(descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new in-memory DBContext for testing purposes
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("BookPlaceControllersTestIMDb");
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

            // Add some default reservations
            IntegrationTestingData.AddDefaultReservations(db);
            
            db.SaveChanges();
        }
    }
}
