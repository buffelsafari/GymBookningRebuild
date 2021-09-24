using GymBooking.Front.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Front.ViewComponents
{
    public class PaginatorComponent:ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync(int numberOfPages, int currentPage)
        {
            var model = new PaginatorModelView
            {                
                NumberOfPages=numberOfPages,
                CurrentPage=currentPage,

            };
            Debug.WriteLine("Hello from paginator c#"+numberOfPages);
            return View(model);
        }
    }
}
