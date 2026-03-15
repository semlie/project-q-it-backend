using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICourseActions
    {
        Task<List<Course>> GetCoursesByClassIdAsync(int classId);
    }
}
