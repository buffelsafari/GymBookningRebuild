using GymBooking.Core.Services.GymClassService;
using GymBooking.Data.Entities;
using GymBooking.Front.Models.ViewModels.GymClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace GymBooking.Front.ViewComponents
{
    
    public class GymClassButtonsComponent:ViewComponent
    {
        private readonly IGymClassService gymClassService;
        private readonly UserManager<ApplicationUser> userManager;
        
        public GymClassButtonsComponent(IGymClassService gymClassService, UserManager<ApplicationUser> userManager)
        {
            this.gymClassService = gymClassService;
            this.userManager = userManager;
            
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime time, int gymClassId)
        {
            bool isBooked = false;
            bool isBookingAvailable = false;
            bool isEditAvailable = false;
            bool isDeleteAvailable = false;
            bool isDetailsAvailable = false;


            if (User.IsInRole("Member"))  
            {
                if (time>DateTime.Now) // todo test date
                {
                    
                    isBookingAvailable = true;
                    var userId = userManager.GetUserId(UserClaimsPrincipal);
                    isBooked = await gymClassService.IsBooked(userId, gymClassId);
                    
                }
                if (User.IsInRole("Admin"))
                {
                    isDeleteAvailable = true;
                    isEditAvailable = true;                
                }
                
                isDetailsAvailable = true;

            }

            
            
            var model = new GymClassButtonsModelView
            {
                GymClassId = gymClassId,
                IsBooked = isBooked,
                IsBookingAvailable = isBookingAvailable,
                IsEditAvailable = isEditAvailable,
                IsDeleteAvailable = isDeleteAvailable,
                IsDetailsAvailable = isDetailsAvailable,
            };

            return View(model);
        }
    }
}
