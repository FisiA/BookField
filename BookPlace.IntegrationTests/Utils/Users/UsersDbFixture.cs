using AutoMapper;
using BookPlace.API.TestingData;
using BookPlace.Core.Domain.Entities;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookPlace.IntegrationTests.Utils.Users
{
    public class UsersDbFixture : IDisposable
    {
        public AppDbContext DbContext { get; private set; }
        public IMapper Mapper { get; private set; }
        public UserManager<User> UserManager { get; private set; }
        public SignInManager<User> SignInManager{ get; private set; }

        private ServiceProvider _serviceProvider;

        public UsersDbFixture()
        {
            var services = new ServiceCollection();

            // Add logging because it is required by UserManager<User> for logging operations (pwd checking etc.)
            services.AddLogging();

            // COnfigure In-Memory DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("UsersServiceTestIMDb")
            );

            // Configure Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure AutoMapper
            var mappingConfig = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>()
            );
            services.AddSingleton(mappingConfig.CreateMapper());

            _serviceProvider = services.BuildServiceProvider();
            DbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            SignInManager = _serviceProvider.GetRequiredService<SignInManager<User>>();
            Mapper = _serviceProvider.GetRequiredService<IMapper>();

            DbContext.Database.EnsureCreated();
            SeedDatabase();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
            _serviceProvider.Dispose();
        }

        private void SeedDatabase()
        {
            // Add default user roles
            IntegrationTestingData.AddDefaultUserRoles(DbContext);

            // Add default users and their roles
            IntegrationTestingData.AddDefaultUsersAndTheirRoles(DbContext);

            DbContext.SaveChanges();
        }
    }
}
