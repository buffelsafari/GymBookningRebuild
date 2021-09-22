using GymBooking.Data.Data;
using GymBooking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Core.Services.GymClassService
{
    

    
    public class GymClassService:IGymClassService
    {
        private GymDbContext context;
        public GymClassService(GymDbContext context)
        {
            this.context = context;
        }

        public IQueryable<GymClassItem> GetGymClassItems(string userId)
        {
            return context.GymClasses.Select(c => new GymClassItem
            {

                Id = c.Id,
                Name = c.Name,
                StartTime = c.StartTime,
                Duration = c.Duration,
                Description = c.Description,
                IsBooked = c.Users.Any(u => u.ApplicationUserId.Equals(userId))
            });
        }

        public async Task<bool> IsBooked(string userId, int gymClassId)
        { 
            return await context.ApplicationUsersGymClasses.FindAsync(userId, gymClassId) !=default;
        }

        public async Task<bool> Toggle(string userId, int gymClassId) 
        {
            var match = await context.ApplicationUsersGymClasses.FindAsync(userId, gymClassId);
            
            if (match != default) //debook
            {
                context.ApplicationUsersGymClasses.Remove(match);
                return false;
            }
            else //book
            {              
                await context.ApplicationUsersGymClasses.AddAsync(new ApplicationUserGymClass 
                { 
                    GymClassId = (await context.GymClasses.FindAsync(gymClassId)).Id,
                    ApplicationUserId = (await context.Users.FindAsync(userId)).Id
                });
                return true;
            }            
        }
    }
}
