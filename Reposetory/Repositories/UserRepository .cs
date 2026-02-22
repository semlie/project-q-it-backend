using Repository.Entities;
using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<Users>
    {
        private readonly IContext _context;
        public UserRepository(IContext context)
        {
               this._context = context;
        }
        public Users AddItem(Users item)
        {
           _context.Set<Users>().Add(item);
            
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            _context.Set<Users>().Remove(GetById(id));
            _context.save();
        }

        public List<Users> GetAll()
        {
           return _context.Set<Users>().ToList();  
        }

        public Users GetById(int id)
        {
            return _context.Set<Users>().FirstOrDefault(x => x.UserId == id);
        }

        public void UpdateItem(int id, Users newItem)
        {
            var user = GetById(id);
            if (user != null)
            {
                user.UserName = newItem.UserName;
                user.UserPassword = newItem.UserPassword;
                user.UserEmail = newItem.UserEmail;
                user.Role = newItem.Role;
                _context.save();
            }
        }
       
    }
}
