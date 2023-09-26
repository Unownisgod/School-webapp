using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;
using System.Data;
using System.Data.Common;

namespace School_webapp.Controllers
{
    public class ClassesController : Controller
    {
        private readonly School_webappContext _context;

        public ClassesController(School_webappContext context)
        {
            _context = context;

        }
        // GET: Classes/Students/5
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Students(int? id)
        {
            MyDbContext context = new MyDbContext();
            //GetStudentList(context);
            if (id == null || _context.StudentClass == null)
            {
                return NotFound();
            }

            var students = await _context.Student
                    .Join(_context.StudentClass,
                          s => s.id, // clave externa de Student
                          c => int.Parse(c.studentId), // clave primaria de StudentClass
                          (s, c) => new { s, c }) // resultado del join
                    .Where(sc => sc.c.classId == id) // filtro por ClassId
                    .Select(sc => sc.s) // selección de los objetos Student
                    .ToListAsync(); // conversión a lista
            if (students == null)
            {
                return NotFound();
            }
            ViewBag.classId = id;
            return View(students);
        }

        // GET: Classes/AddStudents/5
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult AddStudents(int? id)
        {
            using (var context = new MyDbContext())
            {
                GetStudentInfo(context, id);
                ViewBag.classId = id;
                return View();
            }
        }
        // POST: Classes/AddStudent
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> AddStudent(List<int> studentId, int classId)
        {
            if (ModelState.IsValid)
            {
                foreach (var id in studentId)
                {
                    var studentClass = new StudentClass { studentId = id.ToString(), classId = classId };
                    _context.Add(studentClass);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Students", new { id = classId });
            }
            return View(studentId);
        }



        // GET: Classes
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Index()
        {
            using (var context = new MyDbContext())
            {
                GetSubjectList(context);
                GetTeacherList(context);
                return _context.Class != null ?
                          View(await _context.Class.ToListAsync()) :
                          Problem("Entity set 'School_webappContext.Class'  is null.");
            }
        }

        // GET: Classes/Create
        [Authorize(Roles = "Admin, Teacher")]
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
        [Authorize(Roles = "Admin, Teacher")]

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
        [Authorize(Roles = "Admin, Teacher")]

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
        [Authorize(Roles = "Admin, Teacher")]

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

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]

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

        // POST: Classes/Delete/5
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]

        public async Task<IActionResult> StudentDeleteConfirmed(int classId, int id)
        {
            //falta pasar el classId
            if (_context.StudentClass == null)
            {
                return Problem("Entity set 'School_webappContext.StudentClass'  is null.");
            }
            var student = await _context.StudentClass.FirstOrDefaultAsync(sc => int.Parse(sc.studentId) == id && sc.classId == classId);
            if (@student != null)
            {
                _context.StudentClass.Remove(@student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Students", new { id = classId });
        }

        private bool ClassExists(int id)
        {
            return (_context.Class?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private dynamic GetStudentList(MyDbContext context)
        {
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT count(*) FROM studentClass where classId = ";
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
        private dynamic GetTeacherList(MyDbContext context)
        {
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
            command.CommandText = "SELECT teacher.id, teacher.name, lastName FROM teacher join class on teacher.id = class.teacherId";
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
            return ViewBag.teacherList = res;
        }

        private dynamic GetSubjectList(MyDbContext context)
        {
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
            command.CommandText = "SELECT subject.id, subject.name, schoolYear FROM subject join class on subject.id = class.subjectId";
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
            return ViewBag.subjectList = res;
        }
        private dynamic GetStudentInfo(MyDbContext context, int? id)
        {
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //counts table rows     
            command.CommandText = "SELECT COUNT(DISTINCT student.id) FROM student WHERE student.id NOT IN (SELECT StudentClass.studentid FROM StudentClass WHERE StudentClass.classid = " + id + ");";
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable 
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT DISTINCT student.id, student.name, student.lastname FROM student WHERE student.id NOT IN(SELECT StudentClass.studentid FROM StudentClass WHERE StudentClass.classid = " + id + " ); ";
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
            return ViewBag.StudentList = res;
        }
    }
}

internal class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=School_webapp.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}
