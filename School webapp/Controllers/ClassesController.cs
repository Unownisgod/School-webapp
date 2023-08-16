using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class ClassesController : Controller
    {
        private readonly School_webappContext _context;

        public ClassesController(School_webappContext context)
        {
            _context = context;

        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            using (var context = new MyDbContext())
            {
                GetSubjectInfo(context);
                GetTeacherInfo(context);
                return _context.Class != null ?
                          View(await _context.Class.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.Class'  is null.");
            }
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            using (var context = new MyDbContext())
            {
                GetTeacherInfo(context);
                GetSubjectInfo(context);
                return View();
            }

        }
        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,teacherId,SubjectId")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            using (var context = new MyDbContext())
            {
                GetSubjectInfo(context);
                GetTeacherInfo(context);
            }
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,teacherId,SubjectId")] Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
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
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Class == null)
            {
                return Problem("Entity set 'School_webappContext.Class'  is null.");
            }
            var @class = await _context.Class.FindAsync(id);
            if (@class != null)
            {
                _context.Class.Remove(@class);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
          return (_context.Class?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private dynamic GetSubjectInfo(MyDbContext context)
        {
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT count(*) FROM subject";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT id, name, schoolYear FROM subject";
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            string[][] res = new string[count][];
            //reads the data an stores it into the array
            int i = 0;
            while (result.Read())
            {
                res[i] = new string[3];
                res[i][0] = result.GetInt32(0).ToString();
                res[i][1] = result.GetString(1);
                res[i][2] = result.GetInt32(2).ToString();
                i++;
            }
            //stores it into a ViewBag for it to be accessible from the view
            return ViewBag.subject = res;
        }
        private dynamic GetTeacherInfo(MyDbContext context)
        {
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT count(*) FROM teacher";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT id, name, lastName FROM teacher";
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            string[][] res = new string[count][];
            //reads the data an stores it into the array
            int i = 0;
            while (result.Read())
            {
                res[i] = new string[3];
                res[i][0] = result.GetInt32(0).ToString();
                res[i][1] = result.GetString(1);
                res[i][2] = result.GetString(2);
                i++;
            }
            //stores it into a ViewBag for it to be accessible from the view
            return ViewBag.teacher = res;
        }
    }

    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=School_webapp.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
