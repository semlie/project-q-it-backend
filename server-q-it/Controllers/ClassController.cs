using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;
namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IService<Classes> service;
        public ClassController(IService<Classes> service)
        {
            this.service = service;
        }
        [HttpGet]
        public ActionResult<List<Classes>> Get()
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
        public ActionResult<Classes> Get(int id)
        {
            try
            {
                var classItem = service.GetById(id);
                if (classItem == null)
                    return NotFound($"Class with ID {id} not found");
                return Ok(classItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Classes> Post([FromBody] Classes value)
        {
            try
            {
                var entity = new Classes
                {
                    ClassName = value.ClassName,
                    SchoolId = value.SchoolId
                };
                var result = service.AddItem(entity);
                return CreatedAtAction(nameof(Get), new { id = result.ClassId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Classes value)
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
        [HttpGet("school/{id}")]
        public ActionResult<List<Classes>> GetBySchoolId(int id)
        {
            try
            {
                var classes = service.GetAll().Where(c => c.SchoolId == id).ToList();
                if (classes == null || classes.Count == 0)
                    return NotFound($"No classes found for School with ID {id}");
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}