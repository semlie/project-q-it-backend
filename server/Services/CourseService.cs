using AutoMapper;
using Repository.Entities;
using Repository.interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CourseService : IService<Course>, ICourseActions
    {
        private readonly IRepository<Course> _repository;
        private readonly IContext _context;

        public CourseService(IRepository<Course> repository, IContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Course>> getAllAsync()
        {
            return await _repository.getAllAsync();
        }

        public async Task<Course> getByIdAsync(int id)
        {
            return await _repository.getByIdAsync(id);
        }

        public async Task<Course> addItemAsync(Course item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task updateItemAsync(int id, Course item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Course>> GetCoursesByClassIdAsync(int classId)
        {
            var cls = _context.Set<Classes>().FirstOrDefault(c => c.ClassId == classId);
            if (cls == null)
                throw new InvalidOperationException($"Class with ID {classId} not found");

            var courses = (await _repository.getAllAsync()).Where(x => x.SchoolId == cls.SchoolId).ToList();
            return courses;
        }
    }
}
