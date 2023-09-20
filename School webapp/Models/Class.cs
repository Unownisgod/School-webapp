namespace School_webapp.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int teacherId { get; set; }
        public int SubjectId { get; set; }
        public Class() { }

    }
}
