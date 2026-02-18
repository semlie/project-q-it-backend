using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public  class School
    {
        [Key]
        public string NameSchool { get; set; }
        public string NameClass { get; set; }
    }
}
