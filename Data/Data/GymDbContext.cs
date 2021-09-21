using GymBooking.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Data.Data
{
    public class GymDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        DbSet<GymClass> GymClasses { get; set; }
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserGymClass>().HasKey(au => new { au.ApplicationUserId, au.GymClassId });
            
        }

    }
}
