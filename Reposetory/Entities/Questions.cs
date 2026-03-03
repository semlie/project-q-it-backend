using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId {  get; set; }
        public required string Questions { get; set; }
        [Range(1, 3, ErrorMessage = "Level must be 1, 2, or 3")]
        public int Level { get; set; } 
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
    }
}
