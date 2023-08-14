namespace School_webapp.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string tittle { get; set; }
        public string description { get; set; }
        public DateOnly deadline { get; set; }
        public int calification { get; set; }
        public bool isSubmitted { get; set; }
        public bool isRated { get; set; }
        public int subjectId { get; set; }

        public Activity() { }

    }
}
