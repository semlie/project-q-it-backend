using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IService<Chapter> service;
        
        public ChapterController(IService<Chapter> service)
        {
            this.service = service;
        }

        [HttpGet]
        public List<Chapter> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}")]
        public Chapter Get(int id)
        {
            return service.GetById(id);
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Chapter value)
        {
            service.AddItem(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
        [HttpPost]
        public ActionResult<Chapter> Post([FromBody] Chapter value)
        {
            try
            {
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.ChapterId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
