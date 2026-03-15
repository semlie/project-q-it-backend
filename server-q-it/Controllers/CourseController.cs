using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IService<Course> service;
        private readonly IContext _context;
        
        public CourseController(IService<Course> service, IContext context)
        {
            ArgumentNullException.ThrowIfNull(service);
            this.service = service;
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            try
            {
                return Ok(await service.getAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Get(int id)
        {
            try
            {
                var course = await service.getByIdAsync(id);
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
        public async Task<ActionResult<Course>> Post([FromBody] Course value)
        {
            try
            {
                value.CourseId = 0;
                var result = await service.addItemAsync(value);
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
        public async Task<ActionResult> Put(int id, [FromBody] Course value)
        {
            try
            {
                await service.updateItemAsync(id, value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await service.deleteItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("class/{id}")]
        public async Task<ActionResult<List<Course>>> GetByClassId(int id)
        {
            try
            {
                var cls = _context.Set<Classes>().FirstOrDefault(c => c.ClassId == id);
                if (cls == null)
                    return NotFound($"Class with ID {id} not found");
                    
                var courses = (await service.getAllAsync()).Where(x => x.SchoolId == cls.SchoolId).ToList();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
