using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("Tutor")]
    public partial class Tutor
    {
        [Key]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("lastName")]
        public string LastName { get; set; } = null!;
        [Column("phone")]
        public int Phone { get; set; }
        [Column("address")]
        public string Address { get; set; } = null!;
    }
}
