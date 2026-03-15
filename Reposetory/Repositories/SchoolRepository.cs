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
        public async Task<School> AddAsync(School item)
        {
            await _context.Set<School>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var school = await getByIdAsync(id);
            if (school != null)
            {
                _context.Set<School>().Remove(school);
                await _context.saveAsync();
            }
        }

        public async Task<List<School>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<School>().ToList());  
        }

        public async Task<School> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<School>().FirstOrDefault(s => s.SchoolId == id));
        }

        public async Task UpdateAsync(School newItem)
        {
            var school = await getByIdAsync(newItem.SchoolId);
            if (school != null)
            {
                school.NameSchool = newItem.NameSchool;
                await _context.saveAsync();
            }
            else
            {
                throw new Exception($"School with ID {newItem.SchoolId} not found");
            }
        }
    }
}
