using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CourseRepository : IRepository<Course>
    {
        private readonly IContext _context;
        public CourseRepository(IContext context)
        {
               this._context = context;
        }
        public Course AddItem(Course item)
        {
            _context.Courses.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.save();
            }
        }

        public List<Course> GetAll()
        {
           return _context.Courses.ToList();  
        }

        public Course GetById(int id)
        {
            return _context.Courses.FirstOrDefault(x => x.CourseId == id);
        }

        public void UpdateItem(int id, Course item)
        {
            var course =  GetById(id);
            course.CourseName = item.CourseName;
            course.UserId = item.UserId;
            course.UserId = item.UserId;
            _context.save();
        }
    }
}
