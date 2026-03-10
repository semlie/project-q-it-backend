using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    [Table("TestResults")]
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }
        
        [ForeignKey("Users")]
        public int StudentId { get; set; }
        public virtual Users? Student { get; set; }
        
        public string Subject { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public int Duration { get; set; }
    }
}
