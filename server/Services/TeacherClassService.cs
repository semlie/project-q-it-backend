using Repository.Entities;
using Repository.interfaces;
using Service.Interface;

namespace Service.Services
{
    public class TeacherClassService : IService<TeacherClass>
    {
        private readonly IRepository<TeacherClass> repository;

        public TeacherClassService(IRepository<TeacherClass> repository)
        {
            this.repository = repository;
        }

        public async Task<TeacherClass> addItemAsync(TeacherClass item)
        {
            return await repository.AddAsync(item);
        }

        public async Task deleteItemAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<List<TeacherClass>> getAllAsync()
        {
            return await repository.getAllAsync();
        }

        public async Task<TeacherClass> getByIdAsync(int id)
        {
            return await repository.getByIdAsync(id);
        }

        public async Task updateItemAsync(int id, TeacherClass newItem)
        {
            await repository.UpdateAsync(newItem);
        }
    }
}
