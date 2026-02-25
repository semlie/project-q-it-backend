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
        public BDQit(DbContextOptions<BDQit> options) : base(options)
        {
        }

        public virtual DbSet<AnswerOptions> AnswerOptions { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<School> School { get; set; }

        ICollection<Chapter> IContext.Chapters => Chapter.ToList();
        ICollection<Users> IContext.Users => Users.ToList();
        ICollection<Course> IContext.Courses => Course.ToList();
        ICollection<AnswerOptions> IContext.AnswerOptions => AnswerOptions.ToList();
        ICollection<Question> IContext.Questions => Question.ToList();
        ICollection<Materials> IContext.Materials => Materials.ToList();
        ICollection<School> IContext.Schools => School.ToList();

        public void save()
        {
            SaveChanges();
        }
    }
}
