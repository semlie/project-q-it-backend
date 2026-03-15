using Repository.Entities;
using Service.Dto;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAuthActions
    {
        Task<Users?> AuthenticateAsync(UserLogin user);
        Task<Users?> GetUserByIdAsync(int id);
        string GenerateToken(Users user);
        Task<Users?> ValidateTokenAsync(string token);
    }
}
