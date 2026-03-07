using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
        public required string Role { get; set; } // "Student" או "Teacher"
        public string? UserImageUrl { get; set; }
        
        // אם זה תלמיד - יש קישור לכיתה אחת
        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public virtual Classes? Class { get; set; }
        
        // אם זה מורה - יש קישור לכמה כיתות (Many-to-Many)
        public virtual ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}
