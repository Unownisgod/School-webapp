using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class ActivityStudentsController : Controller
    {
        private readonly School_webappContext _context;

        public ActivityStudentsController(School_webappContext context)
        {
            _context = context;
        }

        // GET: ActivityStudents
        public async Task<IActionResult> Index()
        {
            return _context.ActivityStudent != null ?
                        View(await _context.ActivityStudent.ToListAsync()) :
                        Problem("Entity set 'School_webappContext.ActivityStudent'  is null.");
        }

        // GET: ActivityStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActivityStudent == null)
            {
                return NotFound();
            }

            var activityStudent = await _context.ActivityStudent
                .FirstOrDefaultAsync(m => m.activityStudentId == id);
            if (activityStudent == null)
            {
                return NotFound();
            }

            return View(activityStudent);
        }

        // GET: ActivityStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,studentId,activityId,calification,commentary,isSubmitted,isRated,canBeSubmittedLate,isLate,submitDate")] ActivityStudent activityStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityStudent);
        }

        // GET: ActivityStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActivityStudent == null)
            {
                return NotFound();
            }

            var activityStudent = await _context.ActivityStudent.FindAsync(id);
            if (activityStudent == null)
            {
                return NotFound();
            }
            return View(activityStudent);
        }

        // POST: ActivityStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,studentId,activityId,calification,commentary,isSubmitted,isRated,canBeSubmittedLate,isLate,submitDate")] ActivityStudent activityStudent)
        {
            if (id != activityStudent.activityStudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityStudentExists(activityStudent.activityStudentId))
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
            return View(activityStudent);
        }

        // GET: ActivityStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActivityStudent == null)
            {
                return NotFound();
            }

            var activityStudent = await _context.ActivityStudent
                .FirstOrDefaultAsync(m => m.activityId == id);
            if (activityStudent == null)
            {
                return NotFound();
            }

            return View(activityStudent);
        }

        // POST: ActivityStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActivityStudent == null)
            {
                return Problem("Entity set 'School_webappContext.ActivityStudent'  is null.");
            }
            var activityStudent = await _context.ActivityStudent.FindAsync(id);
            if (activityStudent != null)
            {
                _context.ActivityStudent.Remove(activityStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityStudentExists(int id)
        {
            return (_context.ActivityStudent?.Any(e => e.activityStudentId == id)).GetValueOrDefault();
        }
    }
}
