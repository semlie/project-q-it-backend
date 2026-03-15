using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ClassRepository : IRepository<Classes>
    {
        private readonly IContext _context;
        public ClassRepository(IContext context)
        {
               this._context = context;
        }

        public async Task<Classes> AddAsync(Classes item)
        {
            await _context.Set<Classes>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

       
        public async Task DeleteAsync(int id)
        {
            var classItem = await getByIdAsync(id);
            if (classItem != null)
            {
                _context.Set<Classes>().Remove(classItem);
                await _context.saveAsync();
            }
        }
        public async Task<List<Classes>> getAllAsync()
        {
            return await Task.FromResult(_context.Set<Classes>().ToList());
        }
        public async Task<Classes> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Classes>().FirstOrDefault(c => c.ClassId == id));
        }

        public async Task UpdateAsync(Classes item)
        {
            var existingItem = await getByIdAsync(item.ClassId);
            if (existingItem != null)
            {
                existingItem.ClassName = item.ClassName;
                existingItem.SchoolId = item.SchoolId;
                await _context.saveAsync();
            }
            else
            {
                throw new Exception($"Class with ID {item.ClassId} not found");
            }
        }
    }
}
