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
            _context.Set<Course>().Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                _context.Set<Course>().Remove(course);
                _context.save();
            }
        }

        public List<Course> GetAll()
        {
           return _context.Set<Course>().ToList();  
        }
    //החזרת קורס לפי מזהה של course ולא לפי מזהה של school
        public Course GetById(int id)
        {
            return _context.Set<Course>().FirstOrDefault(x => x.CourseId == id);
        }

        public void UpdateItem(int id, Course item)
        {
            var course =  GetById(id);
            course.CourseName = item.CourseName;
            course.ClassId = item.ClassId;
            _context.save();
        }
    }
}
