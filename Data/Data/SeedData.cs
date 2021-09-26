using Bogus;
using GymBooking.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Data.Data
{
    public class SeedData
    {
        private static GymDbContext db;
        private static RoleManager<IdentityRole> roleManager;
        private static UserManager<ApplicationUser> userManager;

        public static async Task InitAsync(GymDbContext context, IServiceProvider services, string adminPW)
        {
            if (string.IsNullOrWhiteSpace(adminPW)) throw new Exception("Cant get password from config");
            if (context is null) throw new NullReferenceException(nameof(GymDbContext));

            db = context;

            if (db.GymClasses.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager is null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            if (userManager is null) throw new NullReferenceException(nameof(UserManager<ApplicationUser>));

            var roleNames = new[] { "Member", "Admin" };
            var adminEmail = "admin@gym.se";

            var gymClasses = GetGymClasses();
            await db.AddRangeAsync(gymClasses);

            await AddRolesAsync(roleNames);

            var admin = await AddAdminAsync(adminEmail, adminPW);
            await AddToRolesAsync(admin, roleNames);

            await db.SaveChangesAsync();
        }

        private static async Task AddToRolesAsync(ApplicationUser admin, string[] roleNames)
        {
            if (admin is null) throw new NullReferenceException(nameof(admin));

            foreach (var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(admin, role)) continue;
                var result = await userManager.AddToRoleAsync(admin, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static IEnumerable<GymClass> GetGymClasses()
        {
            var faker = new Faker("sv");

            var gymClasses = new List<GymClass>();

            for (int i = 0; i < 20; i++)
            {
                var temp = new GymClass
                {
                    
                    Name = faker.Company.CatchPhrase(),
                    Description = faker.Hacker.Verb(),
                    Duration = new TimeSpan(0, 55, 0),
                    StartTime = DateTime.Now.AddDays(faker.Random.Int(-5, 5))                    
                    
                };

                gymClasses.Add(temp);
            }

            return gymClasses;


        }

        private static async Task<ApplicationUser> AddAdminAsync(string adminEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(adminEmail);

            if (found != null) return null;

            var admin = new ApplicationUser
            {
                FirstName = "Adde",
                LastName="Min",
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}
