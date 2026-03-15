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
        public async Task<Course> AddAsync(Course item)
        {
            await _context.Set<Course>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var course = await getByIdAsync(id);
            if (course != null)
            {
                _context.Set<Course>().Remove(course);
                await _context.saveAsync();
            }
        }

        public async Task<List<Course>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<Course>().ToList());  
        }
        
        public async Task<Course> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Course>().FirstOrDefault(x => x.CourseId == id));
        }

        public async Task UpdateAsync(Course item)
        {
            var course = await getByIdAsync(item.CourseId);
            if (course != null)
            {
                course.CourseName = item.CourseName;
                course.SchoolId = item.SchoolId;
                await _context.saveAsync();
            }
        }
    }
}
