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

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            //var userId = userManager.GetUserId(User);            

            var gymClasses=gymClassService.GetGymClassItems()
                .From(DateTime.Now, true)
                .Select(i=> new GymClassIndexItemModelView 
                {
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    //IsBooked = i.IsBooked
                });

            var model = new GymClassIndexModelView
            {
                ViewHistory = false,
                GymClasses = await gymClasses.ToListAsync()
            };

            return View(model);            
        }


        public async Task<IActionResult> History(GymClassIndexModelView inputView)
        {
            Debug.WriteLine("hello from history "+inputView.ViewHistory);

            //var userId = userManager.GetUserId(User);

            var gymClasses = gymClassService.GetGymClassItems()
                .From(DateTime.Now, !inputView.ViewHistory)
                .Select(i => new GymClassIndexItemModelView
                { 
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    //IsBooked = i.IsBooked
                });

            var model = new GymClassIndexModelView
            {
                ViewHistory = inputView.ViewHistory,
                GymClasses = await gymClasses.ToListAsync()
            };

            return View(nameof(Index), model);
        }


        public async Task<IActionResult> BookedClasses()
        {
            var userId = userManager.GetUserId(User);

            var gymClasses = gymClassService.GetBookedGymClassItems(userId)
                .From(DateTime.Now, true)
                .Select(i => new GymClassIndexItemModelView
                {
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    //IsBooked = i.IsBooked
                });

            var model = new GymClassIndexModelView
            {
                ViewHistory = false,
                GymClasses = await gymClasses.ToListAsync()
            };

            return View(model);
        }


        public async Task<IActionResult> BookedHistory()
        {
            var userId = userManager.GetUserId(User);

            var gymClasses = gymClassService.GetBookedGymClassItems(userId)
                .To(DateTime.Now, true)
                .Select(i => new GymClassIndexItemModelView
                {
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    //IsBooked = i.IsBooked
                });

            var model = new GymClassIndexModelView
            {
                ViewHistory = false,
                GymClasses = await gymClasses.ToListAsync()
            };

            return View(model);
        }







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
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> DeleteConfirmed(int id) // not to be used in this app
        {
            
            await gymClassService.RemoveAsync(id);
            
            await gymClassService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       

        [Authorize]
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
