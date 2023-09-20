using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("TeacherStudent")]
    public partial class TeacherStudent
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("studentId")]
        public int StudentId { get; set; }
        [Column("teacherId")]
        public int TeacherId { get; set; }
    }
}
