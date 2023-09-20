namespace SchoolWebapp.Models
{
    public class Event
    {
        internal DateTime Start;

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime DateStart { get; set; }
        public Event()
        {

        }
    }
}
