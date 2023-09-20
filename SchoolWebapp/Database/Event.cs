using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    public partial class Event
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; } = null!;
        [StringLength(50)]
        public string? Tittle { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Start { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Events")]
        public virtual AspNetUser User { get; set; } = null!;
    }
}
