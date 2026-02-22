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
        string path=Directory.GetCurrentDirectory()+"\\images\\";
        public MyMapper() { 
        
            
            CreateMap<Users, UsersDto>().ForMember("Image",x=>x.MapFrom(y=>fromStringToByte(y.UserImageUrl)));
            CreateMap<UsersDto, Users>().ForMember("UserImageUrl",x=>x.MapFrom(y=>y.FileImage.FileName));
        }
        public byte[] fromStringToByte(string mypath)
        {
            return File.ReadAllBytes(path + mypath);
        }
    }
}
