using AutoMapper;
using Repository.Entities;
using Repository.interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ClassService : IService<Classes>, IClassActions
    {
        private readonly IRepository<Classes> _repository;

        public ClassService(IRepository<Classes> repository)
        {
            _repository = repository;
        }

        public async Task<List<Classes>> getAllAsync()
        {
            return await _repository.getAllAsync();
        }

        public async Task<Classes> getByIdAsync(int id)
        {
            return await _repository.getByIdAsync(id);
        }

        public async Task<Classes> addItemAsync(Classes item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task updateItemAsync(int id, Classes item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Classes>> GetClassesBySchoolIdAsync(int schoolId)
        {
            var classes = (await _repository.getAllAsync()).Where(c => c.SchoolId == schoolId).ToList();
            if (classes == null || classes.Count == 0)
                throw new InvalidOperationException($"No classes found for School with ID {schoolId}");
            return classes;
        }
    }
}
