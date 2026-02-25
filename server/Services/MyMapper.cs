using AutoMapper;
using Repository.Entities;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MyMapper :Profile
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "images");
        public MyMapper() { 
        
            
            CreateMap<Users, UsersDto>().ForMember("Image",x=>x.MapFrom(y=>fromStringToByte(y.UserImageUrl)));
            CreateMap<UsersDto, Users>().ForMember("UserImageUrl",x=>x.MapFrom(y=>y.FileImage != null ? y.FileImage.FileName : string.Empty));
        }
        public byte[] fromStringToByte(string? mypath)
        {
            if (string.IsNullOrWhiteSpace(mypath))
            {
                return Array.Empty<byte>();
            }

            var fullPath = Path.Combine(path, mypath);
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : Array.Empty<byte>();
        }
    }
}
