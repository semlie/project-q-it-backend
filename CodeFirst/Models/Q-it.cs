using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=localhost;database=Q-itDB;trusted_connection=true;TrustServerCertificate=True");
            //optionsBuilder.UseSqlite("Data Source=/Users/semliebeskind/Documents/שנה ב/project-q-it/project-q-it-backend/CodeFirst/QitDB.db");
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Q-itDB;User Id=sa;Password=Password12345;TrustServerCertificate=True;");
        }
      /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
}*/
    }
}
