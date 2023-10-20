using School_webapp.Data;
using School_webapp.Models;

namespace SchoolWebapp.Data
{
    public class DataService
    {
        private readonly School_webappContext _context;

        public DataService(School_webappContext context)
        {
            _context = context;
        }

        public List<Class> GetClassesWithStudent(string studentId)
        {
            var resultado = from c in _context.Class
                            join t in _context.StudentClass on c.Id equals t.classId
                            where t.studentId == Convert.ToInt32(studentId)
                            select new { c.Name, c.Id };
            List<Class> classes= new List<Class>();

            foreach (var item in resultado)
            {
                classes.Add(new Class(item.Name, item.Id));
            }
            return classes;
        }

        public List<Class> GetClassesWithTeacher(string teacherId)
        {
            List<Class> classes = new List<Class>();

            if(int.Parse(teacherId) == 0)
            {
                var resultado = from c in _context.Class
                            select new { c.Name, c.Id };

                foreach (var item in resultado)
                {
                    classes.Add(new Class(item.Name, item.Id));
                }
                return classes;
            }
            else{
                var resultado = from c in _context.Class
                            where c.teacherId == Convert.ToInt32(teacherId)
                            select new { c.Name, c.Id };

                foreach (var item in resultado)
                {
                    classes.Add(new Class(item.Name, item.Id));
                }
                return classes;
            }

            
        }

        public List<Teacher> GetTeachers()
        {
            var resultado = from t in _context.Teacher
                            select new {t.id, t.name, t.lastName };
            List<Teacher> teachers = new List<Teacher>();

            foreach (var item in resultado)
            {
                teachers.Add(new Teacher(item.id, item.name, item.lastName));
            }
            return teachers;
        }
        public List<Activity> GetActivitiesOnClass(int classid)
        {
                var resultado = from a in _context.Activity
                                where a.classId == classid
                                select new { a };

                List<Activity> activities = new List<Activity>();

                foreach (var item in resultado)
                {
                    activities.Add(item.a); // Aquí accedemos a la propiedad a
                }
                return activities;

        }
    }
}
