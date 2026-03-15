
using Repository.Entities;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ILogin
    {
        Task<Users?> AuthenticateAsync(UserLogin user);
        Task<Users> GetUserByIdAsync(int id);

    }
}
