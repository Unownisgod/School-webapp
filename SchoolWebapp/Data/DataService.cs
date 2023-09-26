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
            studentId = studentId.Split("-")[0];
            var resultado = from c in _context.Class
                            join t in _context.StudentClass on c.Id equals t.classId
                            where t.studentId.Equals(studentId)
                            select new { c.Name, c.Id };
            List<Class> classes= new List<Class>();

            foreach (var item in resultado)
            {
                classes.Add(new Class(item.Name, item.Id));
            }
            return classes;
        }
    }
}
