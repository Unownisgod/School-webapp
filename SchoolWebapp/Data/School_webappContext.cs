using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace School_webapp.Data
{
    public class School_webappContext : IdentityDbContext
    {
        public School_webappContext()
        {
        }

        public School_webappContext(DbContextOptions<School_webappContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Student> Student { get; set; } = default!;

        public DbSet<Models.Subject>? Subject { get; set; }

        public DbSet<Models.Teacher>? Teacher { get; set; }

        public DbSet<Models.StudentSubject>? StudentSubject { get; set; }

        public DbSet<Models.TeacherStudent>? TeacherStudent { get; set; }

        public DbSet<Models.TeacherSubject>? TeacherSubject { get; set; }

        public DbSet<Models.StudentActivity>? StudentActivity { get; set; }

        public DbSet<Models.Activity>? Activity { get; set; }

        public DbSet<Models.Class>? Class { get; set; }

        public DbSet<Models.StudentClass>? StudentClass { get; set; }

        public DbSet<Models.Tutor>? Tutor { get; set; }

        public DbSet<Models.TutorStudent>? TutorStudent { get; set; }

        public DbSet<Models.ActivityStudent>? ActivityStudent { get; set; }

        public DbSet<SchoolWebapp.Models.Event>? Event { get; set; }
    }
}
