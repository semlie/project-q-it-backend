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
        public async Task<IActionResult> Generate([FromForm] GenerateQuizRequest request)
        {
            if (request?.File == null || request.File.Length == 0)
                return BadRequest("File is required for quiz generation.");
            try
            {
                var result = await _quizService.GenerateQuiz(request);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}