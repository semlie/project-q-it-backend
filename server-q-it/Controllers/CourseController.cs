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
        public CourseController(IService<Course> service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<Course>> Get()
        {
            try
            {
                return Ok(service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Course> Get(int id)
        {
            try
            {
                var course = service.GetById(id);
                if (course == null)
                    return NotFound($"Course with ID {id} not found");
                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Course> Post([FromBody] Course value)
        {
            try
            {
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.CourseId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Course value)
        {
            try
            {
                service.UpdateItem(id, value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                service.DeleteItem(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

