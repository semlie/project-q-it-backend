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
        public Chapter AddItem(Chapter item)
        {
            _context.Set<Chapter>().Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var chapter = GetById(id);
            if (chapter != null)
            {
                _context.Set<Chapter>().Remove(chapter);
                _context.save();
            }
        }

        public List<Chapter> GetAll()
        {
           return _context.Set<Chapter>().ToList();  
        }

        public Chapter GetById(int id)
        {
            return _context.Set<Chapter>().FirstOrDefault(c => c.ChapterId == id);
        }

        public void UpdateItem(int id, Chapter newItem)
        {
            var chapter = GetById(id);
            if (chapter != null)
            {
                chapter.Name = newItem.Name;
                chapter.CourseId = newItem.CourseId;
                _context.save();
            }
        }
    }
}
