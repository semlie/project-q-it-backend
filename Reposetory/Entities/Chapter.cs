using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public  class Chapter
    {
        [Key]
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
    }
}
