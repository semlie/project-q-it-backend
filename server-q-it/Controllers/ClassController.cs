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
                var result = service.AddItem(value);
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
        [HttpGet("unique-names")]
public ActionResult GetUniqName()
{
    try
    {
        var uniqueClasses = service.GetAll()
            .GroupBy(c => c.NameClass)
            .Select(g => g.First())
            .Select(s => new { 
                Id = s.ClassId, 
                NameClass = s.NameClass 
            }).ToList();
        return Ok(uniqueClasses);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}
    }
}