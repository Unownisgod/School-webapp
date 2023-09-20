using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Activity")]
    public partial class Activity
    {
        [Key]
        [Column("activityId")]
        public int ActivityId { get; set; }
        [Column("classId")]
        public int ClassId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column("deadline")]
        public DateTime Deadline { get; set; }
    }
}
