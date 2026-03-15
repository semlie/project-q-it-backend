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
    public class UsersService : IService<Repository.Entities.Users>
    {
        private readonly IRepository<Repository.Entities.Users> repository;
        private readonly IMapper mapper;
        public UsersService(IRepository<Repository.Entities.Users> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Repository.Entities.Users> addItemAsync(Repository.Entities.Users item)
        {
           
           return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<Repository.Entities.Users>> getAllAsync()
        {
           return await repository.getAllAsync();
        }

        public async Task<Repository.Entities.Users> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, Repository.Entities.Users item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
