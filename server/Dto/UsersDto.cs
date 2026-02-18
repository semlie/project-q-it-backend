using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class UsersDto
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string Role { get; set; }
        public required string NameSchool { get; set; }
        public required string NameClass { get; set; }
    }
}
