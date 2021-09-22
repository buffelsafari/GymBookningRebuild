using GymBooking.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymBooking.Data.Data
{
    public class GymDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<ApplicationUserGymClass> ApplicationUsersGymClasses { get; set; }
        
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().Property<DateTime>("TimeOfRegistration");

            builder.Entity<ApplicationUserGymClass>().HasKey(au => new { au.ApplicationUserId, au.GymClassId });
            
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity is ApplicationUser)
                {
                    entry.Property("TimeOfRegistration").CurrentValue = DateTime.Now;
                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
