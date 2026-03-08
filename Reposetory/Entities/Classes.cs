using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    [Table("Classes")]
    public class Classes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }
        public required string ClassName { get; set; }
        
        [ForeignKey("School")]
        public int SchoolId { get; set; }
        public virtual School? School { get; set; }
        
        // תלמידים בכיתה זו
        public virtual ICollection<Users>? Students { get; set; }
        
        // מורים שמלמדים את הכיתה
        public virtual ICollection<TeacherClass>? TeacherClasses { get; set; }
    }
}