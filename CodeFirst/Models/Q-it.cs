using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CodeFirst.Models
{
    public class BDQit:DbContext ,IContext
    {
        public BDQit(DbContextOptions<BDQit> options) : base(options)
        {
        }

        public BDQit() : base()
        {
        }

        public virtual DbSet<AnswerOptions> AnswerOptions { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<TeacherClass> TeacherClass { get; set; }

        ICollection<Chapter> IContext.Chapters => Chapter.ToList();
        ICollection<Users> IContext.Users => Users.ToList();
        ICollection<Course> IContext.Courses => Course.ToList();
        ICollection<AnswerOptions> IContext.AnswerOptions => AnswerOptions.ToList();
        ICollection<Question> IContext.Questions => Question.ToList();
        ICollection<Materials> IContext.Materials => Materials.ToList();
        ICollection<School> IContext.Schools => School.ToList();
        ICollection<Classes> IContext.Classes => Classes.ToList();
        ICollection<TeacherClass> IContext.TeacherClasses => TeacherClass.ToList();

        public void save()
        {
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת יחס Many-to-Many בין מורים לכיתות
            modelBuilder.Entity<TeacherClass>()
                .HasOne(tc => tc.Teacher)
                .WithMany(t => t.TeacherClasses)
                .HasForeignKey(tc => tc.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherClass>()
                .HasOne(tc => tc.Class)
                .WithMany(c => c.TeacherClasses)
                .HasForeignKey(tc => tc.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // הגדרת יחס One-to-Many בין כיתה לתלמידים
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(u => u.ClassId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Question>()
                .ToTable(tableBuilder =>
                {
                    tableBuilder.HasCheckConstraint("CK_Question_Level", "[Level] IN (1, 2, 3)");
                });
        }
    }
}
