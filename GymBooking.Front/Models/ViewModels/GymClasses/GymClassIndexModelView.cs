using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Front.Models.ViewModels.GymClasses
{
    
    public class GymClassIndexModelView
    {
        public bool ViewHistory { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }        
        public string Description { get; set; }
        public IEnumerable<GymClassIndexItemModelView> GymClasses { get; set; }
    }
}
