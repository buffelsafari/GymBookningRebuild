﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBooking.Data.Entities
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration{ get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        public string Description { get; set; }

        public ICollection<ApplicationUserGymClass> Users { get; set; }
    }
}
