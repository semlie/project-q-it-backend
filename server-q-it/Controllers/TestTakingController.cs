using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.interfaces;

namespace webApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestTakingController : ControllerBase
    {
        private readonly IContext _context;

        public TestTakingController(IContext context)
        {
            _context = context;
        }

        [HttpGet("chapter/{chapterId}")]
        public ActionResult GetQuestionsForChapter(int chapterId)
        {
            var questions = _context.Set<Question>().Where(q => q.ChapterId == chapterId).ToList();
            
            var questionList = new List<object>();
            
            foreach (var q in questions)
            {
                var options = _context.Set<AnswerOptions>()
                    .Where(a => a.QuestionId == q.QuestionId)
                    .Select(a => new {
                        id = a.AnswerOptionsId,
                        text = a.Option,
                        isCorrect = a.IsCorrect,
                        description = a.Description
                    })
                    .ToList();

                questionList.Add(new {
                    questionId = q.QuestionId,
                    text = q.Questions,
                    level = q.Level,
                    answers = options
                });
            }

            return Ok(questionList);
        }

        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request)
        {
            try
            {
                var question = _context.Set<Question>().FirstOrDefault(q => q.QuestionId == request.QuestionId);
                if (question == null)
                    return NotFound("Question not found");

                var correctOption = _context.Set<AnswerOptions>()
                    .FirstOrDefault(a => a.QuestionId == request.QuestionId && a.IsCorrect);

                var isCorrect = correctOption != null && 
                    correctOption.AnswerOptionsId.ToString() == request.SelectedAnswerId;

                var attempt = new TestAttempt
                {
                    StudentId = request.StudentId,
                    QuestionId = request.QuestionId,
                    SelectedAnswer = request.SelectedAnswerId,
                    CorrectAnswer = correctOption?.AnswerOptionsId.ToString() ?? "",
                    IsCorrect = isCorrect,
                    AnsweredAt = DateTime.Now,
                    ChapterId = question.ChapterId
                };

                _context.Set<TestAttempt>().Add(attempt);
                await Task.Run(() => _context.save());

                return Ok(new {
                    isCorrect = isCorrect,
                    correctAnswerId = correctOption?.AnswerOptionsId.ToString(),
                    explanation = correctOption?.Description ?? ""
                });
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
                var attempts = _context.Set<TestAttempt>()
                    .Where(a => a.StudentId == request.StudentId && a.ChapterId == request.ChapterId)
                    .ToList();

                var correctCount = attempts.Count(a => a.IsCorrect);
                var totalCount = attempts.Count;

                var chapter = _context.Set<Chapter>().FirstOrDefault(c => c.ChapterId == request.ChapterId);
                var course = chapter != null ? _context.Set<Course>().FirstOrDefault(c => c.CourseId == chapter.CourseId) : null;

                var testResult = new TestResult
                {
                    StudentId = request.StudentId,
                    Subject = course?.CourseName ?? "Unknown",
                    Title = $"מבחן - {chapter?.Name ?? "פרק"}",
                    Date = DateTime.Now,
                    Score = correctCount,
                    MaxScore = totalCount > 0 ? totalCount : 1,
                    Duration = request.Duration,
                    ChapterId = request.ChapterId
                };

                _context.Set<TestResult>().Add(testResult);
                await Task.Run(() => _context.save());

                var percentage = totalCount > 0 ? (correctCount * 100.0 / totalCount) : 0;

                return Ok(new
                {
                    score = correctCount,
                    total = totalCount,
                    percentage = percentage,
                    correctAnswers = attempts.Where(a => a.IsCorrect).Select(a => a.QuestionId).ToList(),
                    wrongAnswers = attempts.Where(a => !a.IsCorrect).Select(a => a.QuestionId).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error finishing test: {ex.Message}");
            }
        }

        [HttpGet("results/{studentId}")]
        public ActionResult GetStudentResults(int studentId)
        {
            var results = _context.Set<TestResult>()
                .Where(t => t.StudentId == studentId)
                .OrderByDescending(t => t.Date)
                .Select(t => new
                {
                    id = t.TestResultId,
                    title = t.Title,
                    subject = t.Subject,
                    score = t.Score,
                    maxScore = t.MaxScore,
                    percentage = t.MaxScore > 0 ? t.Score * 100.0 / t.MaxScore : 0,
                    date = t.Date,
                    duration = t.Duration
                })
                .ToList();

            return Ok(results);
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
    }
}
