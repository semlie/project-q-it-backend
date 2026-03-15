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
    public class ChapterService : IService<Chapter>, IChapterActions
    {
        private readonly IRepository<Chapter> _repository;

        public ChapterService(IRepository<Chapter> repository)
        {
            _repository = repository;
        }

        public async Task<List<Chapter>> getAllAsync()
        {
            return await _repository.getAllAsync();
        }

        public async Task<Chapter> getByIdAsync(int id)
        {
            return await _repository.getByIdAsync(id);
        }

        public async Task<Chapter> addItemAsync(Chapter item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task updateItemAsync(int id, Chapter item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Chapter>> GetChaptersByCourseIdAsync(int courseId)
        {
            var chapter = (await _repository.getAllAsync()).Where(x => x.CourseId == courseId).ToList();
            if (chapter == null || chapter.Count == 0)
                throw new InvalidOperationException($"Chapter with Course ID {courseId} not found");
            return chapter;
        }
    }
}
