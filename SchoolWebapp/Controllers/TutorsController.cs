using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;
using System.Data.Common;

namespace School_webapp.Controllers
{
    public class TutorsController : Controller
    {
        private readonly School_webappContext _context;

        public TutorsController(School_webappContext context)
        {
            _context = context;
        }

        // GET: Tutors
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            return _context.Tutor != null ?
                        View(await _context.Tutor.ToListAsync()) :
                        Problem("Entity set 'School_webappContext.Tutor'  is null.");
        }

        // GET: Tutors/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        // GET: Tutors/AddWards
        [Authorize(Roles = "Admin")]

        public IActionResult AddWards(int? id)
        {
            getStudentList(id);
            ViewBag.Id = id;
            return View();
        }

        // POST: Tutors/AddWards
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddWards(int studentId, int tutorId)
        {
            if (ModelState.IsValid)
            {
                var tutorstudent = new TutorStudent { studentId = studentId, TutorId = tutorId };
                _context.Add(tutorstudent);

                await _context.SaveChangesAsync();
                return RedirectToAction("Wards", new { id = tutorId });
            }
            return View(studentId);
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("Id,name,lastName,phone,address")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutor);
        }

        // GET: Tutors/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tutor == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            return View(tutor);
        }
        // GET: Tutors/Wards/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Wards(int? id)
        {
            ViewBag.tutorId = id;
            if (id == null || _context.Tutor == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            var students = _context.Student.Join(_context.TutorStudent, student => student.id, ts => ts.studentId, (student, ts) =>
                        new { Student = student, TutorStudent = ts }).Join(_context.Tutor, st => st.TutorStudent.TutorId, tutor => tutor.Id, (st, tutor) =>
                        new { Student = st.Student, Tutor = tutor }).Where(s => s.Tutor.Id == id).Select(s => s.Student).ToList();
            return View(students);
        }

        // POST: Classes/Delete/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("DeleteWard")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WardDeleteConfirmed(int tutorId, int id)
        {
            //falta pasar el classId
            if (_context.TutorStudent == null)
            {
                return Problem("Entity set 'School_webappContext.StudentClass'  is null.");
            }
            var ward = await _context.TutorStudent.FirstOrDefaultAsync(sc => sc.studentId == id && sc.TutorId == tutorId);
            if (@ward != null)
            {
                _context.TutorStudent.Remove(@ward);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Wards", new { id = tutorId });
        }
        private dynamic getStudentList(int? id)
        {
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT COUNT(DISTINCT student.id) FROM student WHERE student.id NOT IN (SELECT TutorStudent.studentid FROM TutorStudent WHERE TutorStudent.tutorId = " + id + ");";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT DISTINCT student.id, student.name, student.lastname FROM student WHERE student.id NOT IN(SELECT TutorStudent.studentid FROM TutorStudent WHERE TutorStudent.tutorId = " + id + " );";
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
            return ViewBag.studentList = res;
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,lastName,phone,address")] Tutor tutor)
        {
            if (id != tutor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(tutor.Id))
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
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tutor == null)
            {
                return Problem("Entity set 'School_webappContext.Tutor'  is null.");
            }
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutor.Remove(tutor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorExists(int id)
        {
            return (_context.Tutor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}