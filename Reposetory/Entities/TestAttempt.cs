using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    [Table("TestAttempts")]
    public class TestAttempt
    {
        [Key]
        public int TestAttemptId { get; set; }
        
        [ForeignKey("Users")]
        public int StudentId { get; set; }
        public virtual Users? Student { get; set; }
        
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }
        
        public string SelectedAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }
        
        [ForeignKey("Chapter")]
        public int? ChapterId { get; set; }
        public virtual Chapter? Chapter { get; set; }
    }
}
