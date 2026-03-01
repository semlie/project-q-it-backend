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
        private readonly IService<Users> service;
        private readonly IWebHostEnvironment env;
        
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Users>> Post([FromForm] UsersDto value)
        {
            try
            {
                var imagePath = string.Empty;

                if (value.FileImage is not null && value.FileImage.Length > 0)
                {
                    var imagesDirectory = Path.Combine(env.ContentRootPath, "images");
                    Directory.CreateDirectory(imagesDirectory);

                    var fileExtension = Path.GetExtension(value.FileImage.FileName);
                    var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var fileFullPath = Path.Combine(imagesDirectory, newFileName);

                    await using var stream = new FileStream(fileFullPath, FileMode.Create);
                    await value.FileImage.CopyToAsync(stream);

                    imagePath = Path.Combine("images", newFileName).Replace("\\", "/");
                }

                var user = new Users
                {
                    UserName = value.UserName,
                    UserEmail = value.UserEmail,
                    UserPassword = value.UserPassword,
                    Role = value.Role,
                    SchoolId = value.SchoolId,
                    UserImageUrl = imagePath
                };

                var result = service.AddItem(user);
                return CreatedAtAction(nameof(Get), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
