using Repository.Entities;
using Repository.interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TestTakingService : ITestTakingActions
    {
        private readonly IContext _context;

        public TestTakingService(IContext context)
        {
            _context = context;
        }

        public Task<List<object>> GetQuestionsForChapterAsync(int chapterId, int? level = null)
        {
            var query = _context.Set<Question>().Where(q => q.ChapterId == chapterId);

            if (level.HasValue)
            {
                query = query.Where(q => q.Level == level.Value);
            }

            var questions = query.ToList();

            var questionList = new List<object>();

            foreach (var q in questions)
            {
                var options = _context.Set<AnswerOptions>()
                    .Where(a => a.QuestionId == q.QuestionId)
                    .Select(a => new
                    {
                        id = a.AnswerOptionsId,
                        text = a.Option,
                        isCorrect = a.IsCorrect,
                        description = a.Description ?? ""
                    })
                    .ToList();

                questionList.Add(new
                {
                    questionId = q.QuestionId,
                    text = q.Questions,
                    level = q.Level,
                    answers = options
                });
            }

            return Task.FromResult(questionList);
        }

        public Task<object> SubmitAnswerAsync(int questionId, string selectedAnswerId)
        {
            var question = _context.Set<Question>().FirstOrDefault(q => q.QuestionId == questionId);
            if (question == null)
                throw new InvalidOperationException("Question not found");

            var correctOption = _context.Set<AnswerOptions>()
                .FirstOrDefault(a => a.QuestionId == questionId && a.IsCorrect);

            var isCorrect = correctOption != null &&
                correctOption.AnswerOptionsId.ToString() == selectedAnswerId;

            return Task.FromResult<object>(new
            {
                isCorrect = isCorrect,
                correctAnswerId = correctOption?.AnswerOptionsId.ToString(),
                explanation = correctOption?.Description ?? ""
            });
        }

        public async Task<object> FinishTestAsync(int studentId, int chapterId, int duration, int correctCount, int totalQuestions)
        {
            var chapter = _context.Set<Chapter>().FirstOrDefault(c => c.ChapterId == chapterId);
            var course = chapter != null ? _context.Set<Course>().FirstOrDefault(c => c.CourseId == chapter.CourseId) : null;

            var testResult = new TestResult
            {
                StudentId = studentId,
                Subject = course?.CourseName ?? "Unknown",
                Title = $"מבחן - {chapter?.Name ?? "פרק"}",
                Date = DateTime.Now,
                Score = correctCount,
                MaxScore = totalQuestions > 0 ? totalQuestions : 1,
                Duration = duration
            };

            _context.Set<TestResult>().Add(testResult);
            await _context.saveAsync();

            var percentage = totalQuestions > 0 ? (correctCount * 100.0 / totalQuestions) : 0;

            return new
            {
                score = correctCount,
                total = totalQuestions,
                percentage = percentage
            };
        }

        public Task<List<object>> GetStudentResultsAsync(int studentId)
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

            return Task.FromResult(results.Cast<object>().ToList());
        }
    }
}
