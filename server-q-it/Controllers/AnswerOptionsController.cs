using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnswerOptionsController : ControllerBase
    {
        private readonly IService<AnswerOptions> service;
        
        public AnswerOptionsController(IService<AnswerOptions> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AnswerOptions>>> Get()
        {
            return Ok(await service.getAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerOptions>> Get(int id)
        {
            return Ok(await service.getByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] AnswerOptions value)
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
        public async Task<ActionResult<AnswerOptions>> Post([FromBody] AnswerOptions value)
        {   
            try
            {   
                var result = await service.addItemAsync(value);
                return CreatedAtAction(nameof(Get), new { id = result.AnswerOptionsId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
