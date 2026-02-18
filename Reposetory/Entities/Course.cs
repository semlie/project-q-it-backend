using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("Users")]
        public string CourseName { get; set; }
        public int UserId { get; set; }
    }
}
