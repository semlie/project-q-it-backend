using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly IService<Chapter> service;
        
        public ChapterController(IService<Chapter> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chapter>>> Get()
        {
            return Ok(await service.getAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chapter>> Get(int id)
        {
            var chapter = await service.getByIdAsync(id);
            return Ok(chapter);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Chapter value)
        {
            await service.addItemAsync(value);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await service.deleteItemAsync(id);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Chapter>> Post([FromBody] Chapter value)
        {
            try
            {
                var result = await service.addItemAsync(value);
                return CreatedAtAction(nameof(Get), new { id = result.ChapterId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("course/{id}")]
        public async Task<ActionResult<List<Chapter>>> GetByIdCourse(int id)
        {
            try
            {
                var chapter = (await service.getAllAsync()).Where(x => x.CourseId == id).ToList();
                if (chapter == null || chapter.Count == 0)
                    return NotFound($"Chapter with Course ID {id} not found");
                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
