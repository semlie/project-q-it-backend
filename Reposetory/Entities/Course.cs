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
        public required string CourseName { get; set; }
        [ForeignKey("Classes")]
        public int ClassId { get; set; }
        public virtual Classes? Classes { get; set; }
    }
}
