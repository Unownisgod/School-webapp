using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("TeacherSubject")]
    public partial class TeacherSubject
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("teacherId")]
        public int TeacherId { get; set; }
        [Column("subjectId")]
        public int SubjectId { get; set; }
    }
}
