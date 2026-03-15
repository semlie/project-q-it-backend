using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Dto;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IService<Users> _usersService;
        private readonly IUserActions _userActions;

        public UsersController(IService<Users> usersService, IUserActions userActions)
        {
            _usersService = usersService;
            _userActions = userActions;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            try
            {
                return Ok(await _usersService.getAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving users");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get(int id)
        {
            try
            {
                var user = await _usersService.getByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }

        [HttpGet("{id}/courses")]
        public async Task<ActionResult<List<Course>>> GetUserCourses(int id)
        {
            try
            {
                var courses = await _userActions.GetUserCoursesAsync(id);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving courses: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Users value)
        {
            try
            {
                await _usersService.updateItemAsync(id, value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _usersService.deleteItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user");
            }
        }

        [HttpDelete("{id}/image")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            try
            {
                await _userActions.DeleteUserImageAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Users>> Post([FromForm] UsersDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userActions.CreateUserAsync(value);

                return CreatedAtAction(nameof(Get), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user");
            }
        }
    }
}
