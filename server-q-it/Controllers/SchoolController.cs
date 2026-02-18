using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly IService<School> service;
        
        public SchoolController(IService<School> service)
        {
            this.service = service;
        }

        // GET: api/<SchoolController>
        [HttpGet]
        public List<School> Get()
        {
            return service.GetAll();
        }

        // GET api/<SchoolController>/5
        [HttpGet("{id}")]
        public School Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<SchoolController>
        [HttpPost]
        public School Post([FromBody] School value)
        {
            return service.AddItem(value);
        }

        // PUT api/<SchoolController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] School value)
        {
            service.UpdateItem(id, value);
        }

        // DELETE api/<SchoolController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
