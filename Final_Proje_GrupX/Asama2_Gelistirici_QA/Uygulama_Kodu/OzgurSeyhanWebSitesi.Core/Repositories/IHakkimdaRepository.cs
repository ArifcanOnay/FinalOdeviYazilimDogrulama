using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IHakkimdaRepository : IGenericRepository<Hakkimda>
    {
        Task<Hakkimda?> GetFirstAsync();
    }
}
