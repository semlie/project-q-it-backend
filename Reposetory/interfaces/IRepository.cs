using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.interfaces
{
    public interface IRepository<T>
    {
       Task<List<T>> getAllAsync();
        Task<T> getByIdAsync(int id);
        Task<T> AddAsync(T item);

        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
    }
}
