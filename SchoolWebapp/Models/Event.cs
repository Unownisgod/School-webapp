namespace SchoolWebapp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool? AllDay { get; set; }
        public string? Color { get; set; }
        public string? TextColor { get; set; }
        public string? ClassName { get; set; }
        public string? URL { get; set; }

        public Event()
        {

        }
    }
}
