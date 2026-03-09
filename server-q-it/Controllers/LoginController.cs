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
                return BadRequest("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during login");
            }
        }
        
        [HttpGet("{token}")]
        public IActionResult Get(string token)
        {
            try
            {
                var jwtKey = config["Jwt:Key"];
                var issuer = config["Jwt:Issuer"];
                var audience = config["Jwt:Audience"];

                if (string.IsNullOrWhiteSpace(jwtKey))
                {
                    return StatusCode(500, "JWT configuration is missing");
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
                    return Unauthorized("Invalid token");
                }

                var id = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var user = login.GetUserById(int.Parse(id));
                
                if (user == null)
                {
                    return NotFound("User not found");
                }
                
                return Ok(user);
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized("Token has expired");
            }
            catch (Exception)
            {
                return Unauthorized("Invalid token");
            }
        }
        
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
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier,user1.UserId.ToString()),
            new Claim(ClaimTypes.Name,user1.UserName),
            new Claim(ClaimTypes.Email,user1.UserEmail),
           new Claim(ClaimTypes.Role,user1.Role)
            };
            var token = new JwtSecurityToken(issuer, audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
