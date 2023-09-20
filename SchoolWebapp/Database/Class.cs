using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Class")]
    public partial class Class
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column("teacherId")]
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
