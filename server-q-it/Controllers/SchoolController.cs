using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly IService<School> service;
        
        public SchoolController(IService<School> service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<School>> Get()
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
        public ActionResult<School> Get(int id)
        {
            try
            {
                var school = service.GetById(id);
                if (school == null)
                    return NotFound($"School with ID {id} not found");
                return Ok(school);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<School> Post([FromBody] School value)
        {
            try
            {
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.SchoolId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] School value)
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
    }
}
