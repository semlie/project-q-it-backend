using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IClassActions
    {
        Task<List<Classes>> GetClassesBySchoolIdAsync(int schoolId);
    }
}
