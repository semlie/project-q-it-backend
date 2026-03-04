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
        public required string Role { get; set; }
        public string  UserImageUrl { get; set; }

        [ForeignKey("School")]
        public int SchoolId { get; set; }
    }
}
