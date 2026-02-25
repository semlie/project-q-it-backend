using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class AnswerOptions
    {
        [Key]
        public int  AnswerOptionsId {get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public string Option { get; set; }
        public bool IsCorrect { get; set; }
        public string Description { get; set; }
        //uju

    }
}
