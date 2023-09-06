namespace School_webapp.Models
{
    public class ActivityStudent
    {
        public int activityStudentId { get; set; }
        public int studentId { get; set; }
        public int activityId { get; set; }
        public float calification { get; set; }
        public bool isSubmitted { get; set; }
        public bool isRated { get; set; }
        public bool canBeSubmittedLate { get; set; }
        public bool isLate { get; set; }
        public string? commentary { get; set; }
        public DateTime? submitDate { get; set; }

        public ActivityStudent() { }
    }
}
