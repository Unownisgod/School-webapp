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
    public class TeacherStudentsController : Controller
    {
        private readonly School_webappContext _context;

        public TeacherStudentsController(School_webappContext context)
        {
            _context = context;
        }

        // GET: TeacherStudents
        public async Task<IActionResult> Index()
        {
              return _context.TeacherStudent != null ? 
                          View(await _context.TeacherStudent.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.TeacherStudent'  is null.");
        }

        // GET: TeacherStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeacherStudent == null)
            {
                return NotFound();
            }

            var teacherStudent = await _context.TeacherStudent
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacherStudent == null)
            {
                return NotFound();
            }

            return View(teacherStudent);
        }

        // GET: TeacherStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeacherStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,studentId,teacherId")] TeacherStudent teacherStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacherStudent);
        }

        // GET: TeacherStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeacherStudent == null)
            {
                return NotFound();
            }

            var teacherStudent = await _context.TeacherStudent.FindAsync(id);
            if (teacherStudent == null)
            {
                return NotFound();
            }
            return View(teacherStudent);
        }

        // POST: TeacherStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,studentId,teacherId")] TeacherStudent teacherStudent)
        {
            if (id != teacherStudent.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherStudentExists(teacherStudent.id))
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
            return View(teacherStudent);
        }

        // GET: TeacherStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeacherStudent == null)
            {
                return NotFound();
            }

            var teacherStudent = await _context.TeacherStudent
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacherStudent == null)
            {
                return NotFound();
            }

            return View(teacherStudent);
        }

        // POST: TeacherStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeacherStudent == null)
            {
                return Problem("Entity set 'School_webappContext.TeacherStudent'  is null.");
            }
            var teacherStudent = await _context.TeacherStudent.FindAsync(id);
            if (teacherStudent != null)
            {
                _context.TeacherStudent.Remove(teacherStudent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherStudentExists(int id)
        {
          return (_context.TeacherStudent?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
