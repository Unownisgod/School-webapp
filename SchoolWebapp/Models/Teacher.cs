namespace School_webapp.Models
{
    public class Teacher
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Teacher()
        {

        }

        public Teacher(int id, string name, string lastName)
        {
            this.id = id;
            this.name = name;
            this.lastName = lastName;
        }
    }
}
