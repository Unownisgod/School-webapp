namespace School_webapp.Models
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public int schoolYear { get; set; }
        public char section { get; set; }
        public ICollection<Grades> grades { get; set; } = null!;
        public ICollection<StudentSubject> studentSubjects { get; set; }
        public ICollection<TeacherStudent> teacherStudents { get; set; }
        public Student()
        {

        }
    }
}
