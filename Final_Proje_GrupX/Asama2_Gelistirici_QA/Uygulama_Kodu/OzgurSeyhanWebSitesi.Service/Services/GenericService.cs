using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntitiy
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IUnitOfWorks _unitOfWorks;
        public GenericService(IGenericRepository<T> repository, IUnitOfWorks unitOfWorks)
        {
            _repository = repository;
            _unitOfWorks = unitOfWorks;
        }


        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWorks.CommitAsync();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _unitOfWorks.Commit();
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
            _unitOfWorks.Commit();
        }
    }
}
