﻿using BookPlace.Core.Domain;
using BookPlace.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookPlace.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Reservation> Reservations { get;set; }

        //Override SaveChanges to add audit fields
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        // Set audit fields here
        private void SetAuditFields()
        {
            List<string> excludedEntities = new List<string> { "IdentityRole", "IdentityUserRole" };
            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added && !excludedEntities.Contains(e.Entity.GetType().Name)).ToList();
            addedEntities.ForEach(e =>
            {
                // Uncomment this when we implement User Register/Login
                //e.Property(nameof(BaseEntity.CreateByUserId)).CurrentValue = 
                //e.Property(nameof(BaseEntity.ModifiedByUserId)).CurrentValue = 
                e.Property(nameof(BaseEntity.CreatedOnDate)).CurrentValue = DateTime.UtcNow;
                e.Property(nameof(BaseEntity.ModifiedOnDate)).CurrentValue = DateTime.UtcNow;
                e.Property(nameof(BaseEntity.IsDeleted)).CurrentValue = false;
            });

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && !excludedEntities.Contains(e.Entity.GetType().Name)).ToList();
            modifiedEntities.ForEach(e =>
            {
                // Uncomment this when we implement User Register/Login
                //e.Property(nameof(BaseEntity.ModifiedByUserId)).CurrentValue = 
                e.Property(nameof(BaseEntity.ModifiedOnDate)).CurrentValue = DateTime.UtcNow;
            });
        }
    }
}
