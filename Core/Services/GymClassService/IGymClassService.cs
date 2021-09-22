using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Core.Services.GymClassService
{
    public interface IGymClassService
    {
        IQueryable<GymClassData> GetGymClassItems(string userId);
        IQueryable<GymClassUserData> GetBookedUsers(int gymClassId);
        
        Task<GymClassData> GetGymClassAsync(int gymClassId);
        Task<bool> IsBooked(string userId, int gymClassId);
        Task<bool> Toggle(string userId, int gymClassId);

        

        void Add(GymClassCreationData inputData);
        void Update(GymClassUpdateData inputData);

        Task SaveChangesAsync();
        bool GymClassExists(int gymClassId);
    }
}
