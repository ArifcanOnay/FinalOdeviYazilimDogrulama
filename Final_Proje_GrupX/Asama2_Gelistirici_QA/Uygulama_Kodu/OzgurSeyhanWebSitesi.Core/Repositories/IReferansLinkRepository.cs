using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IReferansLinkRepository : IGenericRepository<ReferansLink>
    {
        List<ReferansLink> GetAllOrderedBySira();
    }
}
