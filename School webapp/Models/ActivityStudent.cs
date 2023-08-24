namespace School_webapp.Models
{
    public class ActivityStudent
    {
        public int Id { get; set; }
        public int studentId { get; set;}
        public int activityId { get; set;}
        public float calification{ get; set; }
        public string commentary{ get; set; }
        public bool isSubmited { get; set; }
        public bool isRated { get; set; }
        public bool canBeSubmitedLate { get; set; }
        public bool isLate { get; set; }
        public DateTime submitDate { get; set; }

        public ActivityStudent() { }
    }
}
