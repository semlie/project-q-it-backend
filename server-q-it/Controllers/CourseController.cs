using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Service.Dto;
using Service.Interface;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IService<Course> service;
        public CourseController(IService<Course> service )
        {
            this.service = service;
        }
        // GET: api/<LoginController>
        [HttpGet]
        public List<Course> Get()
        {
            return service.GetAll();
        }
        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public Course Get(int id)
        {
            return service.GetById(id);
        }
        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Course value)
        {
            service.UpdateItem(id,value);
        }
        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}

