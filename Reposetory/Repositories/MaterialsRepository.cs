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
        public Materials AddItem(Materials item)
        {
            _context.Materials.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var material = GetById(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                _context.save();
            }
        }

        public List<Materials> GetAll()
        {
           return _context.Materials.ToList();  
        }

        public Materials GetById(int id)
        {
            return _context.Materials.FirstOrDefault(m => m.MatId == id);
        }

        public void UpdateItem(int id, Materials item)
        {
            var material = GetById(id);
            if (material != null)
            {     
                material.UserId = item.UserId;
                _context.save();
            }
        }
    }
}
