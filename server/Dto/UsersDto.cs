using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.Dto
{
    public class UsersDto
    {
        public required string UserPassword { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string Role { get; set; }
        public int SchoolId { get; set; }
        public string? NameSchool { get; set; }
        public string? NameClass { get; set; }
         public byte[]? Image { get; set; } //התמונה כמחרוזת
        public IFormFile? FileImage { get; set; }
    }
}
