using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("StudentActivity")]
    public partial class StudentActivity
    {
        [Key]
        public int Id { get; set; }
        [Column("activityId")]
        public int ActivityId { get; set; }
        [Column("studentId")]
        public string StudentId { get; set; } = null!;
    }
}
