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
    public class TutorStudentsController : Controller
    {
        private readonly School_webappContext _context;

        public TutorStudentsController(School_webappContext context)
        {
            _context = context;
        }

        // GET: TutorStudents
        public async Task<IActionResult> Index()
        {
              return _context.TutorStudent != null ? 
                          View(await _context.TutorStudent.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.TutorStudent'  is null.");
        }

        // GET: TutorStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TutorStudent == null)
            {
                return NotFound();
            }

            var tutorStudent = await _context.TutorStudent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutorStudent == null)
            {
                return NotFound();
            }

            return View(tutorStudent);
        }

        // GET: TutorStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TutorStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,studentId,TutorId")] TutorStudent tutorStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutorStudent);
        }

        // GET: TutorStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TutorStudent == null)
            {
                return NotFound();
            }

            var tutorStudent = await _context.TutorStudent.FindAsync(id);
            if (tutorStudent == null)
            {
                return NotFound();
            }
            return View(tutorStudent);
        }

        // POST: TutorStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,studentId,TutorId")] TutorStudent tutorStudent)
        {
            if (id != tutorStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorStudentExists(tutorStudent.Id))
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
            return View(tutorStudent);
        }

        // GET: TutorStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TutorStudent == null)
            {
                return NotFound();
            }

            var tutorStudent = await _context.TutorStudent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutorStudent == null)
            {
                return NotFound();
            }

            return View(tutorStudent);
        }

        // POST: TutorStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TutorStudent == null)
            {
                return Problem("Entity set 'School_webappContext.TutorStudent'  is null.");
            }
            var tutorStudent = await _context.TutorStudent.FindAsync(id);
            if (tutorStudent != null)
            {
                _context.TutorStudent.Remove(tutorStudent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorStudentExists(int id)
        {
          return (_context.TutorStudent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
