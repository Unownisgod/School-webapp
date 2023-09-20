namespace SchoolWebapp.Models
{
    public class Event
    {
        internal DateTime Start;

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public Event()
        {

        }
    }
}
