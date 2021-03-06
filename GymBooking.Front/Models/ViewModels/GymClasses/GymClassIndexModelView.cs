using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Front.Models.ViewModels.GymClasses
{
    
    public class GymClassIndexModelView
    {
        public string ActionEvent { get; set; }
        public bool ViewHistory { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }        
        public string Description { get; set; }
        public IEnumerable<GymClassIndexItemModelView> GymClasses { get; set; }
    }
}
