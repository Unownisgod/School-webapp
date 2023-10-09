namespace School_webapp.Models
{
    public class Activity
    {
        public int activityId { get; set; }
        public int classId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime deadline { get; set; }
        public bool ispublic { get; set; }
        public Activity() { }
    }
}
