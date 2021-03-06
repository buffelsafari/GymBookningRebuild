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

        public IQueryable<GymClassData> GetGymClassItems()
        {
            return context.GymClasses.Select(c => new GymClassData
                {

                    Id = c.Id,
                    Name = c.Name,
                    StartTime = c.StartTime,
                    Duration = c.Duration,
                    Description = c.Description,
                    //IsBooked = c.Users.Any(u => u.ApplicationUserId.Equals(userId))
                });
        }

        public IQueryable<GymClassData> GetBookedGymClassItems(string userId)
        {
            return context.GymClasses.Where(g => g.Users.Any(u => u.ApplicationUserId.Equals(userId)))
                .Select(c => new GymClassData
                {

                    Id = c.Id,
                    Name = c.Name,
                    StartTime = c.StartTime,
                    Duration = c.Duration,
                    Description = c.Description,
                    //IsBooked = true
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


        public IQueryable<GymClassUserData> GetBookedUsers(int gymClassId)
        {
            return context.Users.Where(u => u.GymClasses.Any(c => c.GymClassId == gymClassId))
                .Select(s => new GymClassUserData
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                });
        }

        public bool GymClassExists(int gymClassId)
        {
            return context.GymClasses.Any(e => e.Id == gymClassId);
        }

        public async Task<GymClassData> GetGymClassAsync(int gymClassId)
        {
            var gc=await context.GymClasses.FindAsync(gymClassId);
            return new GymClassData
            {
                Id=gc.Id,
                Name=gc.Name,
                Description=gc.Description,
                Duration=gc.Duration,
                StartTime=gc.StartTime,
            };
        }

        public void Add(GymClassCreationData inputData)
        {
            context.GymClasses.Add(new GymClass
            {
                Name=inputData.Name,
                StartTime=inputData.StartTime,
                Duration=inputData.Duration,
                Description=inputData.Description
            });
        }

        public void Update(GymClassUpdateData inputData)
        {
            context.GymClasses.Update(new GymClass
            {
                Id=inputData.Id,
                Name=inputData.Name,
                StartTime=inputData.StartTime,
                Duration=inputData.Duration,
                Description=inputData.Description
            });
        }

        public async Task RemoveAsync(int gymClassId)
        {
            var gc=await context.GymClasses.FindAsync(gymClassId);
            context.GymClasses.Remove(gc);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
