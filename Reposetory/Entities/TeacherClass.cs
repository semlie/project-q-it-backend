using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class TeacherClass
    {
        [Key]
        public int TeacherClassId { get; set; }
        
        [ForeignKey("Users")]
        public int TeacherId { get; set; }
        public virtual Users? Teacher { get; set; }
        
        [ForeignKey("Classes")]
        public int ClassId { get; set; }
        public virtual Classes? Class { get; set; }
    }
}
