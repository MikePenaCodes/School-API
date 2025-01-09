using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<StudentSubjectGrade> StudentSubjectGrade { get; set; }
        public DbSet<TeacherSubject> TeacherSubject { get; set; }

        // Configure model relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Student-Subject-Grade relationship (combined model)
            modelBuilder.Entity<StudentSubjectGrade>()
                .HasKey(ssg => new { ssg.StudentID, ssg.SubjectID });

            modelBuilder.Entity<StudentSubjectGrade>()
                .HasOne(ssg => ssg.Student)
                .WithMany(s => s.StudentSubjectGrades)
                .HasForeignKey(ssg => ssg.StudentID);

            modelBuilder.Entity<StudentSubjectGrade>()
                .HasOne(ssg => ssg.Subject)
                .WithMany(s => s.StudentSubjectGrades)
                .HasForeignKey(ssg => ssg.SubjectID);

            // Configure Teacher-Subject relationship (many-to-many)
            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => new { ts.TeacherID, ts.SubjectID });

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherID);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectID);

            // You don't need the separate Grade table anymore, since the grade is now part of StudentSubjectGrade
        }
    }
}