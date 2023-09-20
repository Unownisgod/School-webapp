using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Student")]
    public partial class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("lastName")]
        public string LastName { get; set; } = null!;
        [Column("address")]
        public string Address { get; set; } = null!;
        [Column("age")]
        public int Age { get; set; }
        [Column("schoolYear")]
        public int SchoolYear { get; set; }
        [Column("section")]
        [StringLength(1)]
        public string Section { get; set; } = null!;
    }
}
