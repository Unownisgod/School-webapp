using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("StudentSubject")]
    public partial class StudentSubject
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("studentId")]
        public int StudentId { get; set; }
        [Column("subjectId")]
        public int SubjectId { get; set; }
    }
}
