using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("TutorStudent")]
    public partial class TutorStudent
    {
        internal int studentId;

        [Key]
        public int Id { get; set; }
        [Column("studentId")]
        public int StudentId { get; set; }
        public int TutorId { get; set; }
    }
}
