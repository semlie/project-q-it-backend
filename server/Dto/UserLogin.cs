using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class UserLogin
    {
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
        public required string Role { get; set; }
    }
}
