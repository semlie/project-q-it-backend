using AutoMapper;
using Repository.Entities;
using Service.Dto;

namespace Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UsersDto>()
                .ForMember(dest => dest.NameSchool, opt => opt.Ignore())
                .ForMember(dest => dest.NameClass, opt => opt.Ignore());
            
            CreateMap<UsersDto, Users>()
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
