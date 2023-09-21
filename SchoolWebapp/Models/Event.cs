namespace SchoolWebapp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public Event()
        {

        }
    }
}
