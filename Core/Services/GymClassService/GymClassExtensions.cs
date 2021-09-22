using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Core.Services.GymClassService
{
    public static class GymClassExtensions
    {
        public static IQueryable<GymClassData> From(this IQueryable<GymClassData> query, DateTime from, bool active)
        {
            return query.Where(c => !active || c.StartTime > from);
        }

    }
}
