using OzgurSeyhanWebSitesi.Core.Dtos;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IReferansLinkService : IGenericService<ReferansLink>
    {
        List<ReferansLinkDto> GetAllOrderedBySira();
    }
}
