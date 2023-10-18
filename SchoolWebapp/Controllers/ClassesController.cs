using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;
using SchoolWebapp.Models;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System.Net;
using System.IO.Compression;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace School_webapp.Controllers
{
    public class ClassesController : Controller
    {
        private readonly School_webappContext _context;
        UserManager<IdentityUser> _userManager;


        public ClassesController(School_webappContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Activities
        [HttpGet("Identity/Classes/ActivityIndex/{id}")]
        [Authorize]
        public async Task<IActionResult> ActivityIndex()
        {
            return _context.Activity != null ? 
                View(await _context.Activity.ToListAsync()) : 
                Problem("Entity set 'School_webappContext.activity'  is null.");

        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet("Identity/Classes/createActivity/{id}")]

        // GET: Activities/Create
        public IActionResult CreateActivity()
        {
            ViewBag.classId = Request.RouteValues["id"];
            return View();
        }

        [Authorize]
        // GET: Activities/Activity/5
        public IActionResult ActivityStudents(int? id)
        {
            //select the students from the database
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //gets the class
            command.CommandText = "SELECT classId FROM class join Activity on class.id = activity.classid where activity.activityid = " + id;
            context.Database.OpenConnection();
            DbDataReader classId = command.ExecuteReader();
            classId.Read();
            int classid = classId.GetInt32(0);
            classId.Close();
            //counts table rows
            command.CommandText = "SELECT count(*) FROM student join studentClass on student.id = studentclass.studentId where studentClass.classid = " + classid;
            context.Database.OpenConnection();
            DbDataReader counter = command.ExecuteReader();
            //stores it into variaable
            counter.Read();
            int count = counter.GetInt32(0);
            counter.Close();
            //gets relevant values from subject table
            command.CommandText = "SELECT ActivityStudent.isSubmitted, student.name, student.lastName, activitystudent.activitystudentid FROM student JOIN studentClass ON student.id = studentclass.studentId JOIN Activity ON Activity.classId = StudentClass.classId JOIN ActivityStudent ON activity.activityid = ActivityStudent.activityId and student.id = ActivityStudent.studentId where studentClass.classid = " + classid + " and activity.activityId = " + id + " order by 1 desc";
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            string[][] res = new string[count][];
            //reads the data an stores it into the array
            int i = 0;
            while (result.Read())
            {
                res[i] = new string[4];
                res[i][0] = result.GetBoolean(0).ToString();
                res[i][1] = result.GetString(1);
                res[i][2] = result.GetString(2);
                res[i][3] = result.GetInt32(3).ToString();
                i++;
            }
            //stores it into a ViewBag for it to be accessible from the view
            ViewBag.students = res;
            return View();
        }

        [Authorize]
        public IActionResult Activity(int? id)
        {
            //select the students from the database
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier).Split("-")[0];
            //gets the info from the activity and activitystudent tables
            if (User.IsInRole("Student"))
            {
                command.CommandText = "SELECT * FROM Activity join ActivityStudent on activity.activityid = activitystudent.activityid where activitystudent.activityid = " + id + " and activitystudent.studentId = " + userid;
            }
            else
            {
                command.CommandText = "SELECT * FROM Activity join ActivityStudent on activity.activityid = activitystudent.activityid where activitystudent.activitystudentid = " + id;
            }
            context.Database.OpenConnection();
            DbDataReader data = command.ExecuteReader();
            data.Read();

            //get data into classes
            var activity = new Activity
            {
                activityId = data.GetInt32(0),
                classId = data.GetInt32(1),
                Title = data.GetString(2),
                Description = data.GetString(3),
                deadline = data.GetDateTime(4),
            };
            var activityStudent = new ActivityStudent
            {
                activityStudentId = data.GetInt32(6),
                studentId = data.GetInt32(7),
                activityId = data.GetInt32(8),
                calification = data.GetFloat(9),
                isSubmitted = data.GetBoolean(10),
                isRated = data.GetBoolean(11),
                canBeSubmittedLate = data.GetBoolean(12),
                isLate = data.GetBoolean(13),
                commentary = data.IsDBNull(14) ? null : data.GetString(14),
                submitDate = data.IsDBNull(15) ? null : data.GetDateTime(15),
            };
            //stores it into activityviewmodel
            var viewModel = new ActivityViewModel
            {
                Activity = activity,
                ActivityStudent = activityStudent
            };
            data.Close();
            return View(viewModel);
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
        [Authorize(Roles = "Admin, Teacher")]

        public async Task<IActionResult> CreateActivity(ActivityViewModel activityViewModel)
        {

            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            //gets relevant values from subject table
            command.CommandText = "SELECT student.id FROM student join studentClass on student.id = studentclass.studentId where studentClass.classid = " + activityViewModel.Activity.classId;
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            List<ActivityStudent> activityStudentList = new List<ActivityStudent>();
            List<Event> eventList = new List<Event>();
            //reads the data an stores it into the array
            if (ModelState.IsValid)
            {
                _context.Add(activityViewModel.Activity);
                await _context.SaveChangesAsync();
                while (result.Read())
                {
                    activityStudentList.Add(new ActivityStudent
                    {
                        activityId = activityViewModel.Activity.activityId,
                        studentId = result.GetInt32(0),
                        calification = activityViewModel.ActivityStudent.calification,
                        isSubmitted = activityViewModel.ActivityStudent.isSubmitted,
                        isRated = false,
                        canBeSubmittedLate = activityViewModel.ActivityStudent.canBeSubmittedLate,
                        isLate = false,
                        commentary = activityViewModel.ActivityStudent.commentary,
                        submitDate = activityViewModel.ActivityStudent.submitDate,
                    });
                    eventList.Add(new Event
                    {
                        UserId = result.GetInt32(0).ToString() + "-U",
                        Title = activityViewModel.Activity.Title,
                        Start = activityViewModel.Activity.deadline,
                        AllDay = false,
                        Color = "#FF0000",
                        TextColor = "#FFFFFF",
                        ClassName = "event-important"
                    });
                }
                _context.AddRange(activityStudentList);
                _context.AddRange(eventList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ActivityIndex), activityViewModel.Activity.classId);
            }
            return View();
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Admin, Teacher")]

        public async Task<IActionResult> EditActivity(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }
            getClassList();
            var activity = await _context.Activity.FindAsync(id);
            var activityStudent = await _context.ActivityStudent.FindAsync(id);
            ActivityViewModel activityViewModel = new ActivityViewModel(activity, activityStudent);
            if (activityViewModel == null)
            {
                return NotFound();
            }
            return View(activityViewModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivity(int id, ActivityViewModel activityViewModel)
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
            command.CommandText = "SELECT student.id FROM student join studentClass on student.id = studentclass.studentId where studentClass.classid = " + activityViewModel.Activity.classId;
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            List<ActivityStudent> res = new List<ActivityStudent>();
            //reads the data an stores it into the array
            if (ModelState.IsValid)
            {
                activityViewModel.ActivityStudent.activityId = activityViewModel.Activity.activityId = id;
                _context.Update(activityViewModel.Activity);
                await _context.SaveChangesAsync();
                while (result.Read())
                {
                    res.Add(new ActivityStudent
                    {
                        activityId = activityViewModel.Activity.activityId,
                        studentId = result.GetInt32(0),
                        calification = activityViewModel.ActivityStudent.calification,
                        isSubmitted = activityViewModel.ActivityStudent.isSubmitted,
                        isRated = false,
                        canBeSubmittedLate = activityViewModel.ActivityStudent.canBeSubmittedLate,
                        isLate = false,
                        commentary = activityViewModel.ActivityStudent.commentary,
                        submitDate = activityViewModel.ActivityStudent.submitDate,
                    });
                }
                _context.UpdateRange(res);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ActivityIndex), activityViewModel.Activity.classId);
            }
            getClassList();
            return View();
        }

        // POST: Activities/Delete/5
        [Authorize(Roles = "Admin, Teacher")]

        [HttpPost, ActionName("ActivityDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivityDeleteConfirmed(int id)
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

            return RedirectToAction(nameof(ActivityIndex), activity.classId);
        }
        private bool ActivityExists(int id)
        {
            return (_context.Activity?.Any(e => e.activityId == id)).GetValueOrDefault();
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
                          c => Convert.ToInt32(c.studentId), // clave primaria de StudentClass
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
                    var studentClass = new StudentClass { studentId = id, classId = classId };
                    _context.Add(studentClass);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Students", new { id = classId });
            }
            return View();
        }



        // GET: Classes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            using (var context = new MyDbContext())
            {
                GetSubjectList(context);
                GetTeacherList();
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
            return View();
        }

        // GET: Classes/Edit/5
        [Authorize(Roles = "Admin, Teacher")]

        public async Task<IActionResult> Edit(int? id)
        {
            using (var context = new MyDbContext())
            {
                GetSubjectInfo(context);
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
            return View();
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
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
            var student = await _context.StudentClass.FirstOrDefaultAsync(sc => sc.studentId == id && sc.classId == classId);
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
        private dynamic GetTeacherList()
        {
            MyDbContext context = new MyDbContext();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Publish(int? id)
        {
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT * FROM activity where activityId = " + id;
            context.Database.OpenConnection();
            DbDataReader result = command.ExecuteReader();
            //creates an array to store data in
            //store it into object
            result.Read();
            var activity = new Activity
            {
                activityId = result.GetInt32(0),
                classId = result.GetInt32(1),
                Title = result.GetString(2),
                Description = result.GetString(3),
                deadline = result.GetDateTime(4),
                ispublic = true,
            };
            //update database
            _context.Update(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ActivityIndex), activity.classId);

        }
        public async Task<IActionResult> Unpublish(int? id)
        {
            var context = new MyDbContext();
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT * FROM activity where activityId = " + id;
            context.Database.OpenConnection();
            DbDataReader result = await command.ExecuteReaderAsync();
            //creates an array to store data in
            //store it into object
            result.Read();
            var activity = new Activity
            {
                activityId = result.GetInt32(0),
                classId = result.GetInt32(1),
                Title = result.GetString(2),
                Description = result.GetString(3),
                deadline = result.GetDateTime(4),
                ispublic = false,
            };
            //update database
            _context.Update(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ActivityIndex), activity.classId);
        }
        public async Task<IActionResult> UploadFile(int id)
        { // Se comprueba si se ha seleccionado algún archivo
            if (Request.Form.Files.Count > 0)
            {
                string userName = User.Identity.Name;
                // El nombre de usuario
                var user = await _userManager.FindByNameAsync(userName);
                // El objeto de usuario
                var userId = user.Id;
                // Se elimina la carpeta de existir
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", id.ToString(), userId);
                if (Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
                // Se crea la carpeta de no existir
                Directory.CreateDirectory(path);
                // Se obtiene la información del archivo
                foreach (var file in Request.Form.Files)
                {
                    // Se obtiene el nombre del archivo
                    string fileName = Path.GetFileName(file.FileName);
                    // Se construye la ruta donde se guardará el archivo en el servidor
                    string filePath = Path.Combine(path, fileName);
                    // Se guarda el archivo en el servidor
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    // Se muestra un mensaje de confirmación
                    ViewData["Message"] = "El archivo se ha subido correctamente";
                }
                //sets the activity as submitted
                var context = new MyDbContext();
                DbCommand command = context.Database.GetDbConnection().CreateCommand();
                // Separa el -U del userId
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier).Split("-")[0];
                command.CommandText = "SELECT * FROM activitystudent where activityId = " + id + " and studentId = " + userId;
                context.Database.OpenConnection();
                DbDataReader result = await command.ExecuteReaderAsync();
                //creates an array to store data in
                //store it into object
                result.Read();
                var activityStudent = new ActivityStudent
                {
                    activityStudentId = result.GetInt32(0),
                    studentId = result.GetInt32(1),
                    activityId = result.GetInt32(2),
                    calification = result.GetFloat(3),
                    isSubmitted = true,
                    isRated = result.GetBoolean(5),
                    canBeSubmittedLate = result.GetBoolean(6),
                    isLate = result.GetBoolean(7),
                    commentary = result.IsDBNull(8) ? null : result.GetString(8),
                    submitDate = DateTime.Now,
                };
                //update database
                _context.Update(activityStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Activity", "Classes", new { id = activityStudent.activityId });
            }
            else
            {
                // Se muestra un mensaje de error
                ViewData["Message"] = "No se ha seleccionado ningún archivo";
            }
            return View();
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> DownloadFile(int id, int userId)
        {
            // Especificar la ruta local del archivo que quieres descargar
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", id.ToString(), userId.ToString() + "-U");

            // Especificar el nombre del archivo .zip que quieres crear
            string zip = path + ".zip";

            //if zip file exists
            if (System.IO.File.Exists(zip))
            {
                System.IO.File.Delete(zip);
            }

            // Comprimir la carpeta en el archivo .zip con un nivel de compresión óptimo
            ZipFile.CreateFromDirectory(path, zip, CompressionLevel.Optimal, false);

            // Obtener la instancia de HttpResponse
            HttpResponse response = HttpContext.Response;

            // Limpiar el contenido de la respuesta
            response.Clear();

            // Especificar el tipo de contenido como application/zip
            response.ContentType = "application/zip";

            // Especificar el nombre del archivo para la descarga
            response.Headers.Append("Content-Disposition", "attachment; filename=\"" + id.ToString() + "-" + userId.ToString() + "-U.zip\"");

            // Leer el contenido del archivo .zip como un Stream
            using var fileStream = System.IO.File.OpenRead(zip);

            // Obtener un Stream a partir de la tubería de la respuesta
            using var responseStream = response.BodyWriter.AsStream();

            // Copiar el contenido del archivo al Stream de la respuesta de forma asíncrona
            await fileStream.CopyToAsync(responseStream);

            return Redirect(Request.Headers["Referer"].ToString());
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
