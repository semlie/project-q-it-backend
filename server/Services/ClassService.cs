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
        public Classes AddItem(Classes item)
        {
            return repository.AddItem(item);
        }
        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }
        public List<Classes> GetAll()
        {
           return repository.GetAll();
        }
        public Classes GetById(int id)
        {
            return repository.GetById(id);
        }
        public void UpdateItem(int id, Classes item)
        {
            repository.UpdateItem(id,item);
        }
    }
}