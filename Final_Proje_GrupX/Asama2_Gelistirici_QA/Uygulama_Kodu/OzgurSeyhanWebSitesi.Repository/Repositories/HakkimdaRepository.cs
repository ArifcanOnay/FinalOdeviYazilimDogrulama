using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class HakkimdaRepository : GenericRepository<Hakkimda>, IHakkimdaRepository
    {
        public HakkimdaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Hakkimda?> GetFirstAsync()
        {
            return await _context.Hakkimda.FirstOrDefaultAsync();
        }
    }
}
