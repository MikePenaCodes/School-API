using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<StudentSubjectGrade> StudentSubjectGrades { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>()
            .HasKey(s => s.StudentID);

            builder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne() // One-to-one relationship
            .HasForeignKey<Student>(s => s.UserId)  // Foreign key on Student table
            .IsRequired() // User must have a student
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Teacher>()
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<Teacher>(t => t.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            // Define relationships and constraints
            builder.Entity<StudentSubjectGrade>()
                .HasKey(ssg => new { ssg.StudentID, ssg.TeacherSubjectID });

            // Student - StudentSubjectGrade relationship (one-to-many)
            builder.Entity<StudentSubjectGrade>()
                .HasOne(ssg => ssg.Student)
                .WithMany(s => s.StudentSubjectGrades)
                .HasForeignKey(ssg => ssg.StudentID);

            // TeacherSubject - StudentSubjectGrade relationship (one-to-many)
            builder.Entity<StudentSubjectGrade>()
                .HasOne(ssg => ssg.TeacherSubject)
                .WithMany(ts => ts.StudentSubjectGrades)
                .HasForeignKey(ssg => ssg.TeacherSubjectID);

            builder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherID);

            // Subject - TeacherSubject relationship (one-to-many)
            builder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectID);

            List<IdentityRole> roles = new List<IdentityRole>
           {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },
           };
           builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}