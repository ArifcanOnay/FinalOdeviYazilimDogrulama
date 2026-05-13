using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class ReferansLinkRepository : GenericRepository<ReferansLink>, IReferansLinkRepository
    {
        public ReferansLinkRepository(AppDbContext context) : base(context)
        {
        }

        public List<ReferansLink> GetAllOrderedBySira()
        {
            return _context.ReferansLinkler.OrderBy(r => r.Sira).ToList();
        }
    }
}
