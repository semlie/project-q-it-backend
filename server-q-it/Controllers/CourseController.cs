using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IService<Course> service;
        public CourseController(IService<Course> service)
        {
            ArgumentNullException.ThrowIfNull(service);
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
                value.CourseId = 0; // enforce insert
                var result = service.AddItem(value);
                return CreatedAtAction(nameof(Get), new { id = result.CourseId }, result);
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Database update failed: {inner}");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"SQL error {ex.Number}: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
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

        [HttpGet("class/{id}")]
        public ActionResult<List<Course>> GetByClassId(int id)
        {
            try
            {
                // Get the school for this class
                var cls = _context.Set<Classes>().FirstOrDefault(c => c.ClassId == id);
                if (cls == null)
                    return NotFound($"Class with ID {id} not found");
                    
                var courses = service.GetAll().Where(x => x.SchoolId == cls.SchoolId).ToList();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
