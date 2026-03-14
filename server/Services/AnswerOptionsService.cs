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
    public class AnswerOptionsService : IService<AnswerOptions>
    {
        private readonly IRepository<AnswerOptions> repository;
        public AnswerOptionsService(IRepository<AnswerOptions> repository)
        {
            this.repository = repository;
        }
        public async Task<AnswerOptions> addItemAsync(AnswerOptions item)
        {
            return await repository.AddItemAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteItemAsync(id);
        }

        public async Task<List<AnswerOptions>> getAllAsync()
        {
           return await repository.GetAllAsync();
        }

        public async Task<AnswerOptions> getByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task updateItemAsync(int id, AnswerOptions item)
        {
            await repository.UpdateItemAsync(id,item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteItemAsync(id);
        }
    }
}
