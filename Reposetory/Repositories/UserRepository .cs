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
        public async Task<Users> AddAsync(Users item)
        {
           _context.Set<Users>().Add(item);
           await _context.saveAsync();
           return item;
        }
        
        public async Task DeleteAsync(int id)
        {
            var user = await getByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.saveAsync();
            }
        }

        public async Task<List<Users>> getAllAsync()
        {
           return await Task.FromResult(_context.Set<Users>().ToList());  
        }

        public async Task<Users> getByIdAsync(int id)
        {
            return await Task.FromResult(_context.Set<Users>().FirstOrDefault(x => x.UserId == id));
        }
        
        public async Task UpdateAsync(Users newItem)
        {
            var user = await getByIdAsync(newItem.UserId);
            if (user != null)
            {
                user.UserName = newItem.UserName;
                user.UserEmail = newItem.UserEmail;
                user.Role = newItem.Role;
                user.UserImageUrl = newItem.UserImageUrl;
                user.ClassId = newItem.ClassId;
                await _context.saveAsync();
            }
        }
       
    }
}
