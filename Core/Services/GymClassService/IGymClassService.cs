using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Core.Services.GymClassService
{
    public interface IGymClassService
    {
        IQueryable<GymClassItem> GetGymClassItems(string userId);

        Task<bool> IsBooked(string userId, int gymClassId);
        Task<bool> Toggle(string userId, int gymClassId);
    }
}
