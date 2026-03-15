using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ITestTakingActions
    {
        Task<List<object>> GetQuestionsForChapterAsync(int chapterId, int? level = null);
        Task<object> SubmitAnswerAsync(int questionId, string selectedAnswerId);
        Task<object> FinishTestAsync(int studentId, int chapterId, int duration, int correctCount, int totalQuestions);
        Task<List<object>> GetStudentResultsAsync(int studentId);
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
