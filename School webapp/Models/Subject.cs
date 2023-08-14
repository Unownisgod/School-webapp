namespace School_webapp.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int schoolYear { get; set; }
        public ICollection<StudentSubject> studentSubjects { get; set; }
        public ICollection<TeacherSubject> teacherSubjects { get; set; }
        
        public Subject() { 
        }
    }
}
