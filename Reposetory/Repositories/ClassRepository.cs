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
        public Classes AddItem(Classes item)
        {
            _context.Set<Classes>().Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var classItem = GetById(id);
            if (classItem != null)
            {
                _context.Set<Classes>().Remove(classItem);
                _context.save();
            }
        }

        public List<Classes> GetAll()
        {
           return _context.Set<Classes>().ToList();  
        }

        public Classes GetById(int id)
        {
            return _context.Set<Classes>().FirstOrDefault(c => c.ClassId == id);
        }

        public void UpdateItem(int id, Classes newItem)
        {
            var classItem = GetById(id);
            if (classItem != null)
            {
                classItem.NameClass = newItem.NameClass;
                classItem.SchoolId = newItem.SchoolId;
                _context.save();
            }
            else
            {
                throw new Exception($"Class with ID {id} not found");
            }
        }
    }
}
