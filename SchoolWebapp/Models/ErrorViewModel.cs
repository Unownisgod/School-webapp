namespace School_webapp.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
        }

        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}