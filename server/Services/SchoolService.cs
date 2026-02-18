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
    public class SchoolService : IService<School>
    {
        private readonly IRepository<School> repository;
        public SchoolService(IRepository<School> repository)
        {
            this.repository = repository;
        }
        public School AddItem(School item)
        {
            return repository.AddItem(item);
        }
        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }
        public List<School> GetAll()
        {
           return repository.GetAll();
        }
        public School GetById(int id)
        {
            return repository.GetById(id);
        }
        public void UpdateItem(int id, School item)
        {
            repository.UpdateItem(id,item);
        }
    }
}