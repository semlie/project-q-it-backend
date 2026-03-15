using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class MaterialsRepository : IRepository<Materials>
    {
        private readonly IContext _context;
        public MaterialsRepository(IContext context)
        {
               this._context = context;
        }
        public async Task<Materials> AddAsync(Materials item)
        {
            await _context.Set<Materials>().AddAsync(item);
            await _context.saveAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var material = await getByIdAsync(id);
            if (material != null)
            {
                _context.Set<Materials>().Remove(material);
                await _context.saveAsync();
            }
        }

        public async Task<List<Materials>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<Materials>().ToList());  
        }

        public async Task<Materials> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Materials>().FirstOrDefault(m => m.MatId == id));
        }

        public async Task UpdateAsync(Materials item)
        {
            var material = await getByIdAsync(item.MatId);
            if (material != null)
            {
                material.MatName = item.MatName;
                material.MatDescription = item.MatDescription;
                material.MatLink = item.MatLink;
                material.CourseId = item.CourseId;
                await _context.saveAsync();
            }
        }
    }
}
