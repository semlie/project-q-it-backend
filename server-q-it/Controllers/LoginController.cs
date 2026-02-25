using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Service.Dto;
using Service.Interface;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin login;
        private readonly IConfiguration config;
        public LoginController(ILogin login, IConfiguration configuration )
        {
            this.login = login;
            this.config = configuration;
        }
        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] UserLogin user)
        {
            try
            {
                var user1 = login.Authenticate(user);
                if (user1 != null)
                {
                    return Ok(GenerateToken(user1));
                }
                return BadRequest("user not found...");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //יצירת טוקן
        private string GenerateToken(Users user1)
        {
            var jwtKey = config["Jwt:Key"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT configuration is missing. Please set Jwt:Key, Jwt:Issuer and Jwt:Audience.");
            }

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            //אלגוריתם להצפנה
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.Name,user1.UserName),
            new Claim(ClaimTypes.Email,user1.UserEmail),
           new Claim(ClaimTypes.Role,user1.Role)
            //new Claim("Id",user1.Id.ToString()),
            //new Claim(ClaimTypes.GivenName,user1.Name)
            };
            var token = new JwtSecurityToken(issuer, audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
