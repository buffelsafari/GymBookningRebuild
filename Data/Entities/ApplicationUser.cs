using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Data.Entities
{
    public class ApplicationUser:IdentityUser
    {

        public ICollection<ApplicationUserGymClass> GymClasses { get; set; }
    }
}
