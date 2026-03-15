using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.interfaces;
using Service.Interface;
using Service.Dto;
using Service.Services;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IService<Users> service;
        private readonly IWebHostEnvironment env;
        private readonly IContext _context;
        
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        
        public UsersController(IService<Users> service, IWebHostEnvironment env, IContext context)
        {
            this.service = service;
            this.env = env;
            this._context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            try
            {
                return Ok(await service.getAllAsync());
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
                var user = await service.getByIdAsync(id);
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
                var user = await service.getByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found");

                List<Course> courses;

                if (user.Role == "Student")
                {
                    if (user.ClassId.HasValue)
                    {
                        var studentClass = _context.Set<Classes>().FirstOrDefault(c => c.ClassId == user.ClassId.Value);
                        if (studentClass != null)
                        {
                            courses = _context.Set<Course>().Where(c => c.SchoolId == studentClass.SchoolId).ToList();
                            return Ok(courses);
                        }
                        return Ok(new List<Course>());
                    }
                    return Ok(new List<Course>());
                }
                else 
                {
                    var teacherClasses = _context.Set<TeacherClass>().Where(tc => tc.TeacherId == user.UserId).ToList();
                    
                    if (teacherClasses == null || !teacherClasses.Any())
                        return Ok(new List<Course>());

                    var classIds = teacherClasses.Select(tc => tc.ClassId).ToList();
                    var classSchoolIds = _context.Set<Classes>()
                        .Where(c => classIds.Contains(c.ClassId))
                        .Select(c => c.SchoolId)
                        .Distinct()
                        .ToList();

                    courses = _context.Set<Course>()
                        .Where(c => classSchoolIds.Contains(c.SchoolId))
                        .ToList();
                    
                    return Ok(courses);
                }
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
                await service.updateItemAsync(id, value);
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
                await service.deleteItemAsync(id);
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
                var user = await service.getByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                if (!string.IsNullOrWhiteSpace(user.UserImageUrl))
                {
                    var relativePath = user.UserImageUrl.Replace('/', Path.DirectorySeparatorChar);
                    var fileFullPath = Path.Combine(env.ContentRootPath, relativePath);
                    var imagesDirectory = Path.Combine(env.ContentRootPath, "images");

                    if (Path.GetFullPath(fileFullPath).StartsWith(Path.GetFullPath(imagesDirectory), StringComparison.OrdinalIgnoreCase)
                        && System.IO.File.Exists(fileFullPath))
                    {
                        System.IO.File.Delete(fileFullPath);
                    }
                }

                user.UserImageUrl = string.Empty;
                await service.updateItemAsync(id, user);

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
                
                var imagePath = string.Empty;

                if (value.FileImage is not null && value.FileImage.Length > 0)
                {
                    var fileExtension = Path.GetExtension(value.FileImage.FileName).ToLower();
                    if (!AllowedImageExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Only image files (jpg, jpeg, png, gif) are allowed");
                    }
                    
                    var imagesDirectory = Path.Combine(env.ContentRootPath, "images");
                    Directory.CreateDirectory(imagesDirectory);

                    var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var fileFullPath = Path.Combine(imagesDirectory, newFileName);

                    await using var stream = new FileStream(fileFullPath, FileMode.Create);
                    await value.FileImage.CopyToAsync(stream);

                    imagePath = Path.Combine("images", newFileName).Replace("\\", "/");
                }
                
                var hashedPassword = UserLoginService.HashPassword(value.UserPassword);
                
                var user = new Users
                {
                    UserPassword = hashedPassword,
                    UserName = value.UserName,
                    UserEmail = value.UserEmail,
                    Role = value.Role,
                    ClassId = value.ClassId,
                    UserImageUrl = imagePath
                };

                var result = await service.addItemAsync(user);
                
                return CreatedAtAction(nameof(Get), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user");
            }
        }

    }
}
