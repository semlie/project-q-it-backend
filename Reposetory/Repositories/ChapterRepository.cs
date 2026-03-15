using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ChapterRepository : IRepository<Chapter>
    {
        private readonly IContext _context;
        public ChapterRepository(IContext context)
        {
               this._context = context;
        }
        public async Task<Chapter> AddAsync(Chapter item)
        {
            await _context.Set<Chapter>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var chapter = await getByIdAsync(id);
            if (chapter != null)
            {
                _context.Set<Chapter>().Remove(chapter);
                await _context.saveAsync();
            }
        }

        public async Task<List<Chapter>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<Chapter>().ToList());  
        }

        public async Task<Chapter> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Chapter>().FirstOrDefault(c => c.ChapterId == id));
        }

        public async Task UpdateAsync(Chapter newItem)
        {
            var chapter = await getByIdAsync(newItem.ChapterId);
            if (chapter != null)
            {
                chapter.Name = newItem.Name;
                chapter.CourseId = newItem.CourseId;
                await _context.saveAsync();
            }
        }
    }
}
