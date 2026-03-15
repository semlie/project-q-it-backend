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
    public class ChapterService : IService<Chapter>
    {
        private readonly IRepository<Chapter> repository;
        public ChapterService(IRepository<Chapter> repository)
        {
            this.repository = repository;
        }
        public async Task<Chapter> addItemAsync(Chapter item)
        {
            return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<Chapter>> getAllAsync()
        {
           return await repository.getAllAsync();
        }

        public async Task<Chapter> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, Chapter item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
