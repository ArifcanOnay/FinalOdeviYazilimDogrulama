using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntitiy
    {
        protected readonly AppDbContext _context; // protected yaptık
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet=_context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);  
        }

        public void Delete(int id)
        {
            var silinecekDeger = _dbSet.Find(id);
            if (silinecekDeger != null)
            {
                _dbSet.Remove(silinecekDeger);
            }
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
            
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);

        }

       
    }
}
