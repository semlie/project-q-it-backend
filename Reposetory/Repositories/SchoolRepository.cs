using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class SchoolRepository : IRepository<School>
    {
        private readonly IContext _context;
        public SchoolRepository(IContext context)
        {
               this._context = context;
        }
        public School AddItem(School item)
        {
            _context.Set<School>().Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var school = GetById(id);
            if (school != null)
            {
                _context.Set<School>().Remove(school);
                _context.save();
            }
        }

        public List<School> GetAll()
        {
           return _context.Set<School>().ToList();  
        }

        public School GetById(int id)
        {
            return _context.Set<School>().FirstOrDefault(s => s.SchoolId == id);
        }

        public void UpdateItem(int id, School newItem)
        {
            var school = GetById(id);
            if (school != null)
            {
                school.NameSchool = newItem.NameSchool;
                school.NameClass = newItem.NameClass;
                _context.save();
            }
            else
            {
                throw new Exception($"School with ID {id} not found");
            }
        }
    }
}
