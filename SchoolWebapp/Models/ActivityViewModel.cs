namespace School_webapp.Models
{
    public class ActivityViewModel
    {
        public Activity Activity { get; set; }
        public ActivityStudent ActivityStudent { get; set; }
        public ActivityViewModel()
        {

        }

        public ActivityViewModel(Activity act, ActivityStudent actst)
        {
            this.Activity = act;
            this.ActivityStudent = actst;
        }
    }
}
