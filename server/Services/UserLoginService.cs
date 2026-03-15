using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.interfaces;
using Service.Dto;
using Service.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserLoginService : ILogin, IAuthActions
    {
        private readonly IRepository<Users> _repository;
        private readonly IConfiguration _config;

        public UserLoginService(IRepository<Users> repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
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

        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return (await _repository.getAllAsync()).FirstOrDefault(x => x.UserId == id);
        }

        public string GenerateToken(Users user)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT configuration is missing. Please set Jwt:Key, Jwt:Issuer and Jwt:Audience.");
            }

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(issuer, audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Users?> ValidateTokenAsync(string token)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT configuration is missing");
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, validationParameters, out var validatedToken);

            var jwtSecurityToken = validatedToken as JwtSecurityToken;
            if (jwtSecurityToken == null)
            {
                return null;
            }

            var id = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            return await GetUserByIdAsync(int.Parse(id));
        }
    }
}
