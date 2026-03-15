using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    public class TeacherClassDto
    {
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TeacherClassController : ControllerBase
    {
        private readonly IService<TeacherClass> service;
        
        public TeacherClassController(IService<TeacherClass> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeacherClass>>> Get()
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
        public async Task<ActionResult<TeacherClass>> Get(int id)
        {
            try
            {
                var item = await service.getByIdAsync(id);
                if (item == null)
                    return NotFound($"TeacherClass with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TeacherClass>> Post([FromBody] TeacherClassDto value)
        {
            try
            {
                var entity = new TeacherClass
                {
                    TeacherClassId = 0,
                    TeacherId = value.TeacherId,
                    ClassId = value.ClassId,
                    Teacher = null,
                    Class = null
                };
                var result = await service.addItemAsync(entity);
                return CreatedAtAction(nameof(Get), new { id = result.TeacherClassId }, result);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"DB error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TeacherClassDto value)
        {
            try
            {
                var entity = new TeacherClass
                {
                    TeacherId = value.TeacherId,
                    ClassId = value.ClassId,
                    Teacher = null,
                    Class = null
                };
                await service.updateItemAsync(id, entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
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
        [HttpGet("byTeacher/{teacherId}")]
        public async Task<ActionResult<List<TeacherClass>>> GetByTeacher(int teacherId)
        {
            try
            {
                var items = (await service.getAllAsync()).Where(tc => tc.TeacherId == teacherId).ToList();
                if (items == null || items.Count == 0)
                    return NotFound($"No TeacherClass entries found for Teacher ID {teacherId}");
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
