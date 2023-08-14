using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School_webapp.Models;

namespace School_webapp.Data
{
    public class School_webappContext : DbContext
    {
        public School_webappContext (DbContextOptions<School_webappContext> options)
            : base(options)
        {
        }

        public DbSet<School_webapp.Models.Student> Student { get; set; } = default!;

        public DbSet<School_webapp.Models.Subject>? Subject { get; set; }

        public DbSet<School_webapp.Models.Teacher>? Teacher { get; set; }

        public DbSet<School_webapp.Models.StudentSubject>? StudentSubject { get; set; }

        public DbSet<School_webapp.Models.TeacherStudent>? TeacherStudent { get; set; }

        public DbSet<School_webapp.Models.TeacherSubject>? TeacherSubject { get; set; }
    }
}
