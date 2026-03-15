using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;
using System.Collections.Generic;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly IService<Chapter> _chapterService;
        private readonly IChapterActions _chapterActions;

        public ChapterController(IService<Chapter> chapterService, IChapterActions chapterActions)
        {
            _chapterService = chapterService;
            _chapterActions = chapterActions;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chapter>>> Get()
        {
            return Ok(await _chapterService.getAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chapter>> Get(int id)
        {
            var chapter = await _chapterService.getByIdAsync(id);
            return Ok(chapter);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Chapter value)
        {
            await _chapterService.addItemAsync(value);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _chapterService.deleteItemAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Chapter>> Post([FromBody] Chapter value)
        {
            try
            {
                var result = await _chapterService.addItemAsync(value);
                return CreatedAtAction(nameof(Get), new { id = result.ChapterId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("course/{id}")]
        public async Task<ActionResult<List<Chapter>>> GetByIdCourse(int id)
        {
            try
            {
                var chapter = await _chapterActions.GetChaptersByCourseIdAsync(id);
                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
