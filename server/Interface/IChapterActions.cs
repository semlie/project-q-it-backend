using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IChapterActions
    {
        Task<List<Chapter>> GetChaptersByCourseIdAsync(int courseId);
    }
}
