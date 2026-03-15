using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Service.Interface;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IService<Course> _courseService;
        private readonly ICourseActions _courseActions;

        public CourseController(IService<Course> courseService, ICourseActions courseActions)
        {
            _courseService = courseService;
            _courseActions = courseActions;
        }

        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            try
            {
                return Ok(await _courseService.getAllAsync());
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
                var course = await _courseService.getByIdAsync(id);
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
                var result = await _courseService.addItemAsync(value);
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
                await _courseService.updateItemAsync(id, value);
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
                await _courseService.deleteItemAsync(id);
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
                var courses = await _courseActions.GetCoursesByClassIdAsync(id);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
