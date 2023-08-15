namespace School_webapp.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string tittle { get; set; }
        public string description { get; set; }
        public DateTime deadline { get; set; }
        public double calification { get; set; }
        public bool isSubmitted { get; set; }
        public bool isRated { get; set; }
        public bool canBeSubmittedLate { get; set; }
        public bool isLate { get; set; }
        public int classId { get; set; }

        public Activity() { }

    }
}
