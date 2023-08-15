using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class StudentActivitiesController : Controller
    {
        private readonly School_webappContext _context;

        public StudentActivitiesController(School_webappContext context)
        {
            _context = context;
        }

        // GET: StudentActivities
        public async Task<IActionResult> Index()
        {
              return _context.StudentActivity != null ? 
                          View(await _context.StudentActivity.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.StudentActivity'  is null.");
        }

        // GET: StudentActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudentActivity == null)
            {
                return NotFound();
            }

            var studentActivity = await _context.StudentActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentActivity == null)
            {
                return NotFound();
            }

            return View(studentActivity);
        }

        // GET: StudentActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,activityId,studentId")] StudentActivity studentActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentActivity);
        }

        // GET: StudentActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudentActivity == null)
            {
                return NotFound();
            }

            var studentActivity = await _context.StudentActivity.FindAsync(id);
            if (studentActivity == null)
            {
                return NotFound();
            }
            return View(studentActivity);
        }

        // POST: StudentActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,activityId,studentId")] StudentActivity studentActivity)
        {
            if (id != studentActivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentActivityExists(studentActivity.Id))
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
            return View(studentActivity);
        }

        // GET: StudentActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudentActivity == null)
            {
                return NotFound();
            }

            var studentActivity = await _context.StudentActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentActivity == null)
            {
                return NotFound();
            }

            return View(studentActivity);
        }

        // POST: StudentActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudentActivity == null)
            {
                return Problem("Entity set 'School_webappContext.StudentActivity'  is null.");
            }
            var studentActivity = await _context.StudentActivity.FindAsync(id);
            if (studentActivity != null)
            {
                _context.StudentActivity.Remove(studentActivity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentActivityExists(int id)
        {
          return (_context.StudentActivity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
