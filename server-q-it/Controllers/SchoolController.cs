using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        public async Task<ActionResult<List<School>>> Get()
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
        public async Task<ActionResult<School>> Get(int id)
        {
            try
            {
                var school = await service.getByIdAsync(id);
                if (school == null)
                    return NotFound($"School with ID {id} not found");
                return Ok(school);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<School>> Post([FromBody] School value)
        {
            try
            {
                var result = await service.addItemAsync(value);
                return CreatedAtAction(nameof(Get), new { id = result.SchoolId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] School value)
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
        [HttpGet("{schoolName}/classes")]
        public async Task<ActionResult<List<School>>> GetClassesBySchoolName(string schoolName)
        {
            try
            {
                var school = (await service.getAllAsync()).Where(s => s.NameSchool.Equals(schoolName, StringComparison.OrdinalIgnoreCase));
                if (school == null || !school.Any())
                    return NotFound($"School with name {schoolName} not found");
                return Ok(school);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("unique-names")]
        public async Task<ActionResult> GetUniqName()
        {
            try
            {
                var uniqueSchools = (await service.getAllAsync())
                    .GroupBy(s => s.NameSchool)
                    .Select(g => g.First())
                    .Select(s => new { 
                        Id = s.SchoolId, 
                        NameSchool = s.NameSchool 
                    }).ToList();
                return Ok(uniqueSchools);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
