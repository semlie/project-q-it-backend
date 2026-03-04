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
    public class UsersService : IService<Users>
    {
        private readonly IRepository<Users> repository;
        public UsersService(IRepository<Users> repository)
        {
            this.repository = repository;
        }
        public Users AddItem(Users item)
        {
            
           return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<Users> GetAll()
        {
           return repository.GetAll();
        }

        public Users GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, Users item)
        {
            repository.UpdateItem(id, item);
        }
    }
}
