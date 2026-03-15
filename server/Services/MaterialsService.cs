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
    public class MaterialsService : IService<Materials>
    {
        private readonly IRepository<Materials> repository;
        public MaterialsService(IRepository<Materials> repository)
        {
            this.repository = repository;
        }
        public async Task<Materials> addItemAsync(Materials item)
        {
            return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<Materials>> getAllAsync()
        {
           return await repository.getAllAsync();
        }

        public async Task<Materials> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, Materials item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
