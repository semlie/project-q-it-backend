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
        public AnswerOptions AddItem(AnswerOptions item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<AnswerOptions> GetAll()
        {
           return repository.GetAll();
        }

        public AnswerOptions GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, AnswerOptions item)
        {
            repository.UpdateItem(id,item);
        }
    }
}
