namespace School_webapp.Models
{
    public class Class
    {
        public int Id { get; set; }
        ICollection<Student> Students { get; set; }
        ICollection<Subject> Subjects { get; set; }
        public int schoolYear { get; set; }
        public Class() { }

    }
}
