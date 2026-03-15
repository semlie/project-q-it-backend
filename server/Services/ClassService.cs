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
    public class ClassService : IService<Classes>
    {
        private readonly IRepository<Classes> repository;
        public ClassService(IRepository<Classes> repository)
        {
            this.repository = repository;
        }
        public async Task<Classes> addItemAsync(Classes item)
        {
            return await repository.AddAsync(item);
        }
        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }
        public async Task<List<Classes>> getAllAsync()
        {
           return await repository.getAllAsync();
        }
        public async Task<Classes> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }
        public async Task updateItemAsync(int id, Classes item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
