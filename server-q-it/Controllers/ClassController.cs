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
        public async Task<ActionResult<List<Classes>>> Get()
        {
            try
            {
                return Ok(await service.getAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Classes>> Get(int id)
        {
            try
            {
                var classItem = await service.getByIdAsync(id);
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
        public async Task<ActionResult<Classes>> Post([FromBody] Classes value)
        {
            try
            {
                var entity = new Classes
                {
                    ClassName = value.ClassName,
                    SchoolId = value.SchoolId
                };
                var result = await service.addItemAsync(entity);
                return CreatedAtAction(nameof(Get), new { id = result.ClassId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Classes value)
        {
            try
            {
                await service.updateItemAsync(id, value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("school/{id}")]
        public async Task<ActionResult<List<Classes>>> GetBySchoolId(int id)
        {
            try
            {
                var classes = (await service.getAllAsync()).Where(c => c.SchoolId == id).ToList();
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
