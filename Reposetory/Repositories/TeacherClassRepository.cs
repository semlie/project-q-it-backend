using Repository.Entities;
using Repository.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TeacherClassRepository : IRepository<TeacherClass>
    {
        private readonly IContext _context;

        public TeacherClassRepository(IContext context)
        {
            _context = context;
        }

        public async Task<TeacherClass> AddAsync(TeacherClass item)
        {
            item.Teacher = null;
            item.Class = null;
            await _context.Set<TeacherClass>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await getByIdAsync(id);
            if (item != null)
            {
                _context.Set<TeacherClass>().Remove(item);
                await _context.saveAsync();
            }
        }

        public async Task<List<TeacherClass>> getAllAsync()
        {
            return await Task.FromResult(_context.Set<TeacherClass>().ToList());
        }

        public async Task<TeacherClass> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<TeacherClass>().FirstOrDefault(x => x.TeacherClassId == id));
        }

        public async Task UpdateAsync(TeacherClass newItem)
        {
            var item = await getByIdAsync(newItem.TeacherClassId);
            if (item != null)
            {
                item.TeacherId = newItem.TeacherId;
                item.ClassId = newItem.ClassId;
                await _context.saveAsync();
            }
        }
    }
}
