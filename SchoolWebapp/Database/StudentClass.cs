using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("StudentClass")]
    public partial class StudentClass
    {
        [Key]
        public int Id { get; set; }
        [Column("studentId")]
        public int StudentId { get; set; }
        [Column("classId")]
        public int ClassId { get; set; }
    }
}
