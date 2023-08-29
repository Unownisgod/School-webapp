using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly School_webappContext _context;

        public ActivitiesController(School_webappContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
              return _context.Activity != null ? 
                          View(await _context.Activity.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.Activity'  is null.");
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.activityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            getClassList();
            return View();
        }

        private dynamic getClassList()
        {
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT count(*) FROM class";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT id, name FROM class";
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            string[][] res = new string[count][];
            //reads the data an stores it into the array
            int i = 0;
            while (result.Read())
            {
                res[i] = new string[2];
                res[i][0] = result.GetInt32(0).ToString();
                res[i][1] = result.GetString(1);
                i++;
            }
            //stores it into a ViewBag for it to be accessible from the view
            return ViewBag.classes = res;
        }
    

    // POST: Activities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel activityViewModel)
        {
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT count(*) FROM student";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT student.id FROM student join studentClass on student.id = studentclass.studentId where studentClass.classid = "+activityViewModel.Activity.classId;
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            List<ActivityStudent> res = new List<ActivityStudent>();
            //reads the data an stores it into the array
            if (ModelState.IsValid)
            {
                _context.Add(activityViewModel.Activity);
                await _context.SaveChangesAsync();
                while (result.Read())
                {
                    res.Add(new ActivityStudent { activityId = activityViewModel.Activity.activityId, 
                        studentId=result.GetInt32(0),
                        calification = activityViewModel.ActivityStudent.calification,
                        isSubmitted = activityViewModel.ActivityStudent.isSubmitted,
                        isRated = false,
                        canBeSubmittedLate = activityViewModel.ActivityStudent.canBeSubmittedLate,
                        isLate = false,
                        commentary = activityViewModel.ActivityStudent.commentary,
                        submitDate = activityViewModel.ActivityStudent.submitDate,
                    });
                }
                _context.AddRange(res);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            getClassList();
            return View(activityViewModel.Activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,classId,Title,Description,deadline")] Activity activity)
        {
            if (id != activity.activityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.activityId))
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
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.activityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'School_webappContext.Activity'  is null.");
            }
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activity?.Any(e => e.activityId == id)).GetValueOrDefault();
        }
    }
}
