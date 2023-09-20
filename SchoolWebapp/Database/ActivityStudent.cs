using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolWebapp.Database
{
    [Table("ActivityStudent")]
    public partial class ActivityStudent
    {
        [Key]
        [Column("activityStudentId")]
        public int ActivityStudentId { get; set; }
        [Column("studentId")]
        public int StudentId { get; set; }
        [Column("activityId")]
        public int ActivityId { get; set; }
        [Column("calification")]
        public float Calification { get; set; }
        [Column("isSubmitted")]
        public bool IsSubmitted { get; set; }
        [Column("isRated")]
        public bool IsRated { get; set; }
        [Column("canBeSubmittedLate")]
        public bool CanBeSubmittedLate { get; set; }
        [Column("isLate")]
        public bool IsLate { get; set; }
        [Column("commentary")]
        public string? Commentary { get; set; }
        [Column("submitDate")]
        public DateTime? SubmitDate { get; set; }
    }
}
