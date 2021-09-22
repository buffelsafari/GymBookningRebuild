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
        private readonly GymDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGymClassService gymClassService;

        public GymClassesController(GymDbContext context, UserManager<ApplicationUser> userManager, IGymClassService gymClassService)
        {
            this.context = context;
            this.userManager = userManager;
            this.gymClassService = gymClassService;
        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);            

            var gymClasses=gymClassService.GetGymClassItems(userId)
                .From(DateTime.Now)
                .Select(i=> new GymClassIndexItemModelView 
                {
                    Id = i.Id,
                    Name = i.Name,
                    StartTime = i.StartTime,
                    Duration = i.Duration,
                    Description = i.Description,
                    IsBooked = i.IsBooked
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
                        
            var gymClass=await gymClassService.GetGymClass((int)id);
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
                await gymClassService.AddAsync(new GymClassCreationData 
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

            var gymClass = await context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(gymClass);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!gymClassService.GymClassExists(gymClass.Id))
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
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await context.GymClasses.FindAsync(id);
            context.GymClasses.Remove(gymClass);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool GymClassExists(int id)
        //{
        //    return context.GymClasses.Any(e => e.Id == id);
        //}



        [Authorize]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = userManager.GetUserId(User);                        

            await gymClassService.Toggle(userId, (int)id);            

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
