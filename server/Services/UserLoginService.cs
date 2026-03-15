using Repository.Entities;
using Repository.interfaces;
using Service.Dto;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserLoginService : ILogin
    {
        private readonly IRepository<Users> _repository;
        public UserLoginService(IRepository<Users> _repository)
        {
            this._repository = _repository;
        }
        public async Task<Users?> AuthenticateAsync(UserLogin user)
        {
            var existingUser = (await _repository.getAllAsync()).FirstOrDefault(x => x.UserEmail == user.UserEmail);
            if (existingUser == null)
            {
                return null;
            }
            
            if (BCrypt.Net.BCrypt.Verify(user.UserPassword, existingUser.UserPassword))
            {
                return existingUser;
            }
            return null;
        }
        
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        
        public async Task<Users> GetUserByIdAsync(int id)
        {
            return (await _repository.getAllAsync()).FirstOrDefault(x => x.UserId == id);
        }
    }
}
