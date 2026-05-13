using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IGenericRepository<T>where T : class
    {
        List<T> GetAll();
        Task<T?> GetByIdAsync(int id);
       void Update(T entity);
        void Delete(int id);  
        Task AddAsync(T entity);
    }
}
