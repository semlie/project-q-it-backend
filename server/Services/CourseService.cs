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
        public async Task<Course> addItemAsync(Course item)
        {
            return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<Course>> getAllAsync()
        {
           return await repository.getAllAsync();
        }

        public async Task<Course> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, Course item)
        {
            await repository.UpdateAsync(item);
        }
    }
}
