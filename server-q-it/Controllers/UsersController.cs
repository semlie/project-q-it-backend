using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
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
        
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        
        public UsersController(IService<Users> service, IWebHostEnvironment env)
        {
            this.service = service;
            this.env = env;
        }
        
        [HttpGet]
        public ActionResult<List<Users>> Get()
        {
            try
            {
                return Ok(service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving users");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Users> Get(int id)
        {
            try
            {
                var user = service.GetById(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Users value)
        {
            try
            {
                service.UpdateItem(id, value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                service.DeleteItem(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user");
            }
        }

        [HttpDelete("{id}/image")]
        public ActionResult DeleteImage(int id)
        {
            try
            {
                var user = service.GetById(id);
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
                service.UpdateItem(id, user);

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

                var result = service.AddItem(user);
                
                if (value.Role == "Teacher" && value.ClassIds != null && value.ClassIds.Any())
                {
                }
                
                return CreatedAtAction(nameof(Get), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user");
            }
        }

    }
}
