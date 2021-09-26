using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBooking.Data.Data;
using GymBooking.Data.Entities;
using Microsoft.AspNetCore.Identity;
using GymBooking.Front.Models.ViewModels.GymClasses;
using GymBooking.Core.Services.GymClassService;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace GymBooking.Front.Controllers
{


    public class GymClassesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGymClassService gymClassService;

        public GymClassesController(UserManager<ApplicationUser> userManager, IGymClassService gymClassService)
        {
            this.userManager = userManager;
            this.gymClassService = gymClassService;
        }



        private async Task<GymClassIndexModelView> GetIndexModelView(IQueryable<GymClassData> baseQuery, int pageSize, int currentPage, bool viewHistory, string actionEvent)
        {

            var itemCount = baseQuery
                .Count();

            var gymClasses = baseQuery
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .Select(i => new GymClassIndexItemModelView
                {
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    //IsBooked = i.IsBooked
                });

            return new GymClassIndexModelView
            {
                ActionEvent = actionEvent,
                ViewHistory = viewHistory,
                NumberOfPages = itemCount / pageSize + (itemCount % pageSize == 0 ? 0 : 1),
                CurrentPage = currentPage,
                GymClasses = await gymClasses.ToListAsync()
            };

        }



        public async Task<ActionResult> OnIndexChange(string buttonValue, string viewHistory)
        {
            bool vHistory = false;
            bool.TryParse(viewHistory, out vHistory);
            int pageSize = 3;// settings
            int result = 1;
            if (int.TryParse(buttonValue, out result))
            {
                result = result < 1 ? 1 : result;
            }
            else
            {
                result = 1;
            }

            var baseQuery = gymClassService.GetGymClassItems()
                .From(DateTime.Now, !vHistory);

            var model = await GetIndexModelView(baseQuery, pageSize, result, vHistory, "OnIndexChange");

            return PartialView("GymClassTablePartial", model);

        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            var baseQuery = gymClassService.GetGymClassItems()
                .From(DateTime.Now, true);

            var model = await GetIndexModelView(baseQuery, 3, 1, false, "OnIndexChange");

            return View(model);
        }



        [Authorize(Roles ="Member")]
        
        public async Task<ActionResult> OnBookedClassesChange(string buttonValue, string viewHistory)
        {
            bool vHistory = false;
            bool.TryParse(viewHistory, out vHistory);
            int pageSize = 3;// settings
            int result = 1;
            if (int.TryParse(buttonValue, out result))
            {
                result = result < 1 ? 1 : result;
            }
            else
            {
                result = 1;
            }

            var userId = userManager.GetUserId(User);

            var baseQuery = gymClassService.GetBookedGymClassItems(userId)
               .From(DateTime.Now, true);

            var model = await GetIndexModelView(baseQuery, pageSize, result, vHistory, "OnBookedClassesChange");

            return PartialView("GymClassTablePartial", model);

        }

        [Authorize(Roles ="Member")]        
        public async Task<IActionResult> BookedClasses()
        {
           
            var userId = userManager.GetUserId(User);

            var baseQuery = gymClassService.GetBookedGymClassItems(userId)
               .From(DateTime.Now, true);

            var model = await GetIndexModelView(baseQuery, 3, 1, false, "OnBookedClassesChange");


            return View(model);
        }


        [Authorize(Roles ="Member")]
        public async Task<ActionResult> OnBookedHistoryChange(string buttonValue, string viewHistory)
        {
            bool vHistory = false;
            bool.TryParse(viewHistory, out vHistory);
            int pageSize = 3;// settings
            int result = 1;
            if (int.TryParse(buttonValue, out result))
            {
                result = result < 1 ? 1 : result;
            }
            else
            {
                result = 1;
            }

            var userId = userManager.GetUserId(User);

            var baseQuery = gymClassService.GetBookedGymClassItems(userId)
                .To(DateTime.Now, true);
               

            var model = await GetIndexModelView(baseQuery, pageSize, result, vHistory, "OnBookedHistoryChange");

            return PartialView("GymClassTablePartial", model);

        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> BookedHistory()
        {
            
            var userId = userManager.GetUserId(User);

            var baseQuery = gymClassService.GetBookedGymClassItems(userId)
                .To(DateTime.Now, true);

            var model = await GetIndexModelView(baseQuery, 3, 1, false, "OnBookedHistoryChange");

            return View(model);
        }






        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                        
            var gymClass=await gymClassService.GetGymClassAsync((int)id);
            if (gymClass==null)
            {
                return NotFound();
            }
                        
            var userQ=gymClassService.GetBookedUsers((int)id)
            
                .Select(s=>new GymClassDetailsItemModelView 
                { 
                    UserId=s.Id,
                    FirstName=s.FirstName,
                    LastName=s.LastName
                });

            

            GymClassDetailsModelView model = new GymClassDetailsModelView
            {
                Id = gymClass.Id,
                Name = gymClass.Name,
                StartTime = gymClass.StartTime,
                Duration = gymClass.Duration,
                Description = gymClass.Description,
                Users = await userQ.ToListAsync()

            };


            return View(model);
        }








        // GET: GymClasses/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(GymClassCreateModelView modelView)
        {
            if (ModelState.IsValid)
            {
                gymClassService.Add(new GymClassCreationData 
                { 
                    Name=modelView.Name,
                    StartTime=modelView.StartTime,
                    Duration=modelView.Duration,
                    Description=modelView.Description
                });                
                await gymClassService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modelView);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass=await gymClassService.GetGymClassAsync((int)id);
            //var gymClass = await context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            var model = new GymClassUpdateModelView 
            { 
                Id=gymClass.Id,
                Name=gymClass.Name,
                StartTime=gymClass.StartTime,
                Duration=gymClass.Duration,
                Description=gymClass.Description
            };

            return View(model);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id, GymClassUpdateModelView modelView)
        {
            if (id != modelView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    gymClassService.Update(new GymClassUpdateData 
                    { 
                        Id=modelView.Id,
                        Name=modelView.Name,
                        StartTime=modelView.StartTime,
                        Duration=modelView.Duration,
                        Description=modelView.Description
                    });
                    await gymClassService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!gymClassService.GymClassExists(modelView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(modelView);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var gymClass = await gymClassService.GetGymClassAsync((int)id);                
            if (gymClass==null)
            {
                return NotFound();
            }

            var model = new GymClassDeleteModelView
            { 
                Id=gymClass.Id,
                Name=gymClass.Name,
                StartTime=gymClass.StartTime,
                Duration=gymClass.Duration,
                Description=gymClass.Description
            };


            return View(model);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id) // not to be used in this app
        {
            
            await gymClassService.RemoveAsync(id);
            
            await gymClassService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = userManager.GetUserId(User);                        

            await gymClassService.Toggle(userId, (int)id);            

            await gymClassService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
                        
        }

    }
}
