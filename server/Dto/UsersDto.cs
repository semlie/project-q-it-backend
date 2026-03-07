using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Repository.Entities;

namespace Service.Dto
{
    public class UsersDto
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
        public int? ClassId { get; set; }
        public List<int>? ClassIds { get; set; }
        public IFormFile? FileImage { get; set; }
    }
}
