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
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string UserEmail { get; set; } = string.Empty;
        public string? UserPassword { get; set; }
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty; // "Student" or "Teacher"
        [MaxLength(500)]
        public string? UserImageUrl { get; set; }
        
        // If student - link to one class
        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public virtual Classes? Class { get; set; }
        
        // If teacher - link to multiple classes (Many-to-Many)
        public virtual ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}
