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
    public class CourseService : IService<Course>
    {
        private readonly IRepository<Course> repository;
        public CourseService(IRepository<Course> repository)
        {
            this.repository = repository;
        }
        public Course AddItem(Course item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<Course> GetAll()
        {
           return repository.GetAll();
        }

        public Course GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, Course item)
        {
            repository.UpdateItem(id,item);
        }
    }
}
