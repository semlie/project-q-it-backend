using Repository.Entities;
using Service.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserActions
    {
        Task<List<Course>> GetUserCoursesAsync(int userId);
        Task DeleteUserImageAsync(int id);
        Task<Users> CreateUserAsync(UsersDto userDto);
    }
}
