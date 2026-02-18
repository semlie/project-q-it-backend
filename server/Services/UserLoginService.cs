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
        public Users Authenticate(UserLogin user)
        {
         return   _repository.GetAll().FirstOrDefault(x => x.UserEmail == user.Email && x.UserName == user.UserName);
        }
    }
}
