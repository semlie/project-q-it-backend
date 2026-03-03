using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Materials
    {
        [Key]
        public int MatId { get; set; }
        public required string MatName { get; set; }
        public string? MatDescription { get; set; }
        public required string MatLink { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
    }
}
