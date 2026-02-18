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
        public string Questions { get; set; }
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
    }
}
