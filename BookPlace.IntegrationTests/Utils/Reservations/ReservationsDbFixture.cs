using AutoMapper;
using BookPlace.API.TestingData;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BookPlace.IntegrationTests.Utils
{
    public class ReservationsDbFixture : IDisposable
    {
        public AppDbContext DbContext { get; private set; }
        public IMapper Mapper { get; private set; }

        public ReservationsDbFixture()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ReservationsServiceTestIMDb")
                .Options;

            DbContext = new AppDbContext(options);
            DbContext.Database.EnsureCreated();
            SeedDatabase();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = config.CreateMapper();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }

        private void SeedDatabase()
        {
            // Add default user roles
            IntegrationTestingData.AddDefaultUserRoles(DbContext);

            // Add default users and their roles
            IntegrationTestingData.AddDefaultUsersAndTheirRoles(DbContext);

            // Add some default reservations
            IntegrationTestingData.AddDefaultReservations(DbContext);

            DbContext.SaveChanges();
        }
    }
}
