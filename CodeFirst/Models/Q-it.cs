using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models
{
    public class BDQit:DbContext ,IContext
    {
        public virtual DbSet<AnswerOptions> AnswerOptions { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<School> School { get; set; }

        IEnumerable<Chapter> IContext.Chapters => Chapter;
        IEnumerable<Users> IContext.Users => Users;
        IEnumerable<Course> IContext.Courses => Course;
        IEnumerable<AnswerOptions> IContext.AnswerOptions => AnswerOptions;
        IEnumerable<Question> IContext.Questions => Question;
        IEnumerable<Materials> IContext.Materials => Materials;
        IEnumerable<School> IContext.Schools => School;

        public void save()
        {
            SaveChanges();
        }

        public void AddSchool(School school)
        {
            School.Add(school);
        }

        public void RemoveSchool(School school)
        {
            School.Remove(school);
        }

        public void AddCourse(Course course)
        {
            Course.Add(course);
        }

        public void RemoveCourse(Course course)
        {
            Course.Remove(course);
        }

        public void AddUser(Users user)
        {
            Users.Add(user);
        }

        public void RemoveUser(Users user)
        {
            Users.Remove(user);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=sql;database=Q-itDB;trusted_connection=true;TrustServerCertificate=True");
            optionsBuilder.UseSqlServer("Server=.;Database=QitDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
