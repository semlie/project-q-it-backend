using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.interfaces
{
    public interface IContext
    {
        public IEnumerable<School> Schools { get; }
        public IEnumerable<Users> Users { get; }
        public IEnumerable<Materials> Materials { get; }
        public IEnumerable<Question> Questions { get; }
        public IEnumerable<AnswerOptions> AnswerOptions { get; }
        public IEnumerable<Chapter> Chapters { get; }
        public IEnumerable<Course> Courses { get; }

        public void save();
        public void AddSchool(School school);
        public void RemoveSchool(School school);
        public void AddCourse(Course course);
        public void RemoveCourse(Course course);
        public void AddUser(Users user);
        public void RemoveUser(Users user);
    }
}
