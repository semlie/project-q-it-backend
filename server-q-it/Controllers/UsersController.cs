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
        
        public UsersController(IService<Users> service)
        {
            this.service = service;
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
        [HttpPost]
        public ActionResult<Users> Post([FromBody] Users value)
        {
            try
            {
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
