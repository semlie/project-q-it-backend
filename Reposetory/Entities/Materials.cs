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
        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
