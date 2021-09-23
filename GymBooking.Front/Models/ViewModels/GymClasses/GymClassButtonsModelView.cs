using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Front.Models.ViewModels.GymClasses
{
    public class GymClassButtonsModelView
    {
        public int GymClassId { get; set; }
        public bool IsBooked { get; set; }
        public bool IsBookingAvailable { get; set; }        

        public bool IsDeleteAvailable { get; set; }
        public bool IsDetailsAvailable { get; set; }
        public bool IsEditAvailable { get; set; }
        
    }
}
