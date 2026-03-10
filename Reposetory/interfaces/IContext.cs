using Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.interfaces
{
    public interface IContext
    {
        public ICollection<School> Schools { get; }
        public ICollection<Users> Users { get; }
        public ICollection<Materials> Materials { get; }
        public ICollection<Question> Questions { get; }
        public ICollection<AnswerOptions> AnswerOptions { get; }
        public ICollection<Chapter> Chapters { get; }
        public ICollection<Course> Courses { get; }
        public ICollection<Classes> Classes { get; }
        public ICollection<TeacherClass> TeacherClasses { get; }
        public ICollection<TestResult> TestResults { get; }
        public ICollection<TestAttempt> TestAttempts { get; }
        public DbSet<T> Set<T>() where T : class;
        public void save();
    }
}
