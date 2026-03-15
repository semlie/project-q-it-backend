using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Collections.Generic;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestTakingController : ControllerBase
    {
        private readonly ITestTakingActions _testTakingActions;

        public TestTakingController(ITestTakingActions testTakingActions)
        {
            _testTakingActions = testTakingActions;
        }

        [HttpGet("chapter/{chapterId}")]
        [HttpGet("chapter/{chapterId}/level/{level}")]
        public async Task<ActionResult<List<object>>> GetQuestionsForChapter(int chapterId, int? level = null)
        {
            try
            {
                var questions = await _testTakingActions.GetQuestionsForChapterAsync(chapterId, level);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving questions: {ex.Message}");
            }
        }

        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request)
        {
            try
            {
                var result = await _testTakingActions.SubmitAnswerAsync(request.QuestionId, request.SelectedAnswerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error submitting answer: {ex.Message}");
            }
        }

        [HttpPost("finish")]
        public async Task<IActionResult> FinishTest([FromBody] FinishTestRequest request)
        {
            try
            {
                var result = await _testTakingActions.FinishTestAsync(
                    request.StudentId,
                    request.ChapterId,
                    request.Duration,
                    request.CorrectCount,
                    request.TotalQuestions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error finishing test: {ex.Message}");
            }
        }

        [HttpGet("results/{studentId}")]
        public async Task<ActionResult<List<object>>> GetStudentResults(int studentId)
        {
            try
            {
                var results = await _testTakingActions.GetStudentResultsAsync(studentId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving results: {ex.Message}");
            }
        }
    }

    public class SubmitAnswerRequest
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedAnswerId { get; set; } = "";
    }

    public class FinishTestRequest
    {
        public int StudentId { get; set; }
        public int ChapterId { get; set; }
        public int Duration { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
    }
}
