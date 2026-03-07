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

        public TeacherClass AddItem(TeacherClass item)
        {
            return repository.AddItem(item);
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public List<TeacherClass> GetAll()
        {
            return repository.GetAll();
        }

        public TeacherClass GetById(int id)
        {
            return repository.GetById(id);
        }

        public void UpdateItem(int id, TeacherClass newItem)
        {
            repository.UpdateItem(id, newItem);
        }
    }
}
