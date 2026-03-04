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
        public Question AddItem(Question item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<Question> GetAll()
        {
           return repository.GetAll();
        }

        public Question GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, Question item)
        {
            repository.UpdateItem(id,item);
        }
    }
}
