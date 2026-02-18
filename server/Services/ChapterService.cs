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
        public Chapter AddItem(Chapter item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<Chapter> GetAll()
        {
           return repository.GetAll();
        }

        public Chapter GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, Chapter item)
        {
            repository.UpdateItem(id,item);
        }
    }
}
