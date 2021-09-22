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

        Task<GymClassData> GetGymClass(int gymClassId);

        Task<bool> IsBooked(string userId, int gymClassId);
        Task<bool> Toggle(string userId, int gymClassId);

        IQueryable<GymClassUserData> GetBookedUsers(int gymClassId);

        Task AddAsync(GymClassCreationData inputData);

        Task SaveChangesAsync();
        bool GymClassExists(int gymClassId);
    }
}
