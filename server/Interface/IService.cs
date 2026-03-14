using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IService<T>
    {
        Task<List<T>> getAllAsync();
        Task<T> getByIdAsync(int id);
        Task<T> addItemAsync(T item);
        Task updateItemAsync(int id,T item);
        Task deleteItemAsync(int id);
    }
}
