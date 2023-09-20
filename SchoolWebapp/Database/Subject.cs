using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Subject")]
    public partial class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column("schoolYear")]
        public int SchoolYear { get; set; }
    }
}
