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
            return await repository.AddAsync(item);
        }
        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<AnswerOptions>> getAllAsync()
        {
            return await repository.getAllAsync();
        }

        public async Task<AnswerOptions> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, AnswerOptions item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
