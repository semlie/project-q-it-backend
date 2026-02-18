using AutoMapper;
using Repository.Entities;
using Repository.interfaces;
using Service.Dto;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UsersService : IService<UsersDto>
    {
        private readonly IRepository<Users> repository;
        private readonly IMapper mapper;
        public UsersService(IRepository<Users> repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public UsersDto AddItem(UsersDto item)
        {

           return mapper.Map<Users,UsersDto>( repository.AddItem(mapper.Map<UsersDto, Users>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.GetById(id);
        }

        public List<UsersDto> GetAll()
        {
           return mapper.Map<List<Users>,List<UsersDto>>(repository.GetAll());
        }

        public UsersDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int id, UsersDto item)
        {
            throw new NotImplementedException();
        }
    }
}
