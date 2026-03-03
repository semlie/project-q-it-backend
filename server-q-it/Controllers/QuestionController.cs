using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IService<Question> service;
        
        public QuestionController(IService<Question> service)
        {
            this.service = service;
        }

        [HttpGet]
        public List<Question> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}")]
        public Question Get(int id)
        {
            return service.GetById(id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Question value)
        {
            var existingQuestion = service.GetById(id);
            if (existingQuestion == null)
            {
                return NotFound();
            }

            service.UpdateItem(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
        [HttpPost]
        public ActionResult<Question> Post([FromBody] Question value)
        {
            try
            {
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.QuestionId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
