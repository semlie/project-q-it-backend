using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interface;
using static Service.Interface.IQuizService;
namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateQuizRequest request)
        {
            if (request == null)
                return BadRequest("Text is required for quiz generation.");
            try
            {
                var quiz = await _quizService.GenerateQuiz(request);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}