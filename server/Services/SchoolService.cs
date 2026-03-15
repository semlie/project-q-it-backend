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
    public class SchoolService : IService<School>
    {
        private readonly IRepository<School> repository;
        public SchoolService(IRepository<School> repository)
        {
            this.repository = repository;
        }
        public async Task<School> addItemAsync(School item)
        {
            return await repository.AddAsync(item);
        }
        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }
        public async Task<List<School>> getAllAsync()
        {
           return await repository.getAllAsync();
        }
        public async Task<School> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }
        public async Task updateItemAsync(int id, School item)
        {
            await repository.UpdateAsync(item);
        }
    }
}