using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Teacher")]
    public partial class Teacher
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("lastName")]
        public string LastName { get; set; } = null!;
        [Column("age")]
        public int Age { get; set; }
        [Column("email")]
        public string Email { get; set; } = null!;
        [Column("phone")]
        public string Phone { get; set; } = null!;
        [Column("address")]
        public string Address { get; set; } = null!;
    }
}
