﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<School_webapp.Models.Student> Student { get; set; } = default!;

        public DbSet<School_webapp.Models.Subject>? Subject { get; set; }

        public DbSet<School_webapp.Models.Teacher>? Teacher { get; set; }

        public DbSet<School_webapp.Models.StudentSubject>? StudentSubject { get; set; }

        public DbSet<School_webapp.Models.TeacherStudent>? TeacherStudent { get; set; }

        public DbSet<School_webapp.Models.TeacherSubject>? TeacherSubject { get; set; }

        public DbSet<School_webapp.Models.StudentActivity>? StudentActivity { get; set; }

        public DbSet<School_webapp.Models.Activity>? Activity { get; set; }

        public DbSet<School_webapp.Models.Class>? Class { get; set; }

        public DbSet<School_webapp.Models.StudentClass>? StudentClass { get; set; }

        public DbSet<School_webapp.Models.Tutor>? Tutor { get; set; }

        public DbSet<School_webapp.Models.TutorStudent>? TutorStudent { get; set; }

        public DbSet<School_webapp.Models.ActivityStudent>? ActivityStudent { get; set; }

        public DbSet<SchoolWebapp.Models.Event>? Event { get; set; }
    }
}
