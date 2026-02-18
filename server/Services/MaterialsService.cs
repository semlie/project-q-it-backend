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
        public Materials AddItem(Materials item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<Materials> GetAll()
        {
           return repository.GetAll();
        }

        public Materials GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, Materials item)
        {
            repository.UpdateItem(id,item);
        }
    }
}
