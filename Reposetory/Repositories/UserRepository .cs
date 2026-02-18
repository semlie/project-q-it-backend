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
           _context.Users.Add(item);
            
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            _context.Users.Remove(GetById(id));
            _context.save();
        }

        public List<Users> GetAll()
        {
           return _context.Users.ToList();  
        }

        public Users GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.UserId == id);
        }

        public void UpdateItem(int id, Users item)
        {
          var User=  GetById(id);
            User.UserName = item.UserName;
            User.UserEmail = item.UserEmail;
            User.Role = item.Role;
            User.NameSchool = item.NameSchool;
            User.NameClass = item.NameClass;
            _context.save();
        }
    }
}
