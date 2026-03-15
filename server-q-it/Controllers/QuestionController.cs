using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IService<Question> service;
        
        public QuestionController(IService<Question> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Question>>> Get()
        {
            return Ok(await service.getAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> Get(int id)
        {
            return Ok(await service.getByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Question value)
        {
            var existingQuestion = await service.getByIdAsync(id);
            if (existingQuestion == null)
            {
                return NotFound();
            }

            await service.updateItemAsync(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await service.deleteItemAsync(id);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Question>> Post([FromBody] Question value)
        {
            try
            {
                var result = await service.addItemAsync(value);
                return CreatedAtAction(nameof(Get), new { id = result.QuestionId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
