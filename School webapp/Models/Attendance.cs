namespace School_webapp.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateOnly date { get; set; }
        public int period { get; set; }
        public bool attended { get; set; }
        public bool late { get; set; }
        public Attendance()
        {
            
        }
    }
}
