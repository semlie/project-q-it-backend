using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerOptionsController : ControllerBase
    {
        private readonly IService<AnswerOptions> service;
        
        public AnswerOptionsController(IService<AnswerOptions> service)
        {
            this.service = service;
        }

        [HttpGet]
        public List<AnswerOptions> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}")]
        public AnswerOptions Get(int id)
        {
            return service.GetById(id);
        }

        [HttpPut("{id}")]
        public void Put([FromBody] AnswerOptions value)
        {
            service.AddItem(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
