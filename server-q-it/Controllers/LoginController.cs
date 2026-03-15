using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Service.Dto;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthActions _authActions;

        public LoginController(IAuthActions authActions)
        {
            _authActions = authActions;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserLogin user)
        {
            try
            {
                var user1 = await _authActions.AuthenticateAsync(user);
                if (user1 != null)
                {
                    return Ok(_authActions.GenerateToken(user1));
                }
                return BadRequest("Invalid credentials");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during login");
            }
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> Get(string token)
        {
            try
            {
                var user = await _authActions.ValidateTokenAsync(token);

                if (user == null)
                {
                    return Unauthorized("Invalid token");
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
    }
}
