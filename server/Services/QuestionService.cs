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
    public class QuestionService : IService<Question>
    {
        private readonly IRepository<Question> repository;
        public QuestionService(IRepository<Question> repository)
        {
            this.repository = repository;
        }
        public async Task<Question> addItemAsync(Question item)
        {
            return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<Question>> getAllAsync()
        {
           return await repository.getAllAsync();
        }

        public async Task<Question> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, Question item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
