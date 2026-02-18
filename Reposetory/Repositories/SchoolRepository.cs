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
            _context.Schools.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var school = GetById(id);
            if (school != null)
            {
                _context.Schools.Remove(school);
                _context.save();
            }
        }

        public List<School> GetAll()
        {
           return _context.Schools.ToList();  
        }

        public School GetById(int id)
        {
            return _context.Schools.FirstOrDefault(s => s.NameSchool == id.ToString());
        }

        public void UpdateItem(int id, School item)
        {
            var school = GetById(id);
            if (school != null)
            {
                school.NameSchool = item.NameSchool;
                school.NameClass = item.NameClass;
                _context.save();
            }
        }
    }
}
