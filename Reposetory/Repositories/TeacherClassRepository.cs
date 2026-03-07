using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class TeacherClassRepository : IRepository<TeacherClass>
    {
        private readonly IContext _context;

        public TeacherClassRepository(IContext context)
        {
            _context = context;
        }

        public TeacherClass AddItem(TeacherClass item)
        {
            _context.TeacherClasses.Add(item);
            _context.save();
            return item;
        }

        public void DeleteItem(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _context.TeacherClasses.Remove(item);
                _context.save();
            }
        }

        public List<TeacherClass> GetAll()
        {
            return _context.TeacherClasses.ToList();
        }

        public TeacherClass GetById(int id)
        {
            return _context.TeacherClasses.FirstOrDefault(x => x.TeacherClassId == id);
        }

        public void UpdateItem(int id, TeacherClass newItem)
        {
            var item = GetById(id);
            if (item != null)
            {
                item.TeacherId = newItem.TeacherId;
                item.ClassId = newItem.ClassId;
                _context.save();
            }
        }
    }
}
