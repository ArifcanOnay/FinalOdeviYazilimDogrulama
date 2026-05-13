using OzgurSeyhanWebSitesi.Core.Dtos.HakkimdaDtos;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IHakkimdaService : IGenericService<Hakkimda>
    {
        Task<HakkimdaDto?> GetHakkimdaAsync();
        Task<HakkimdaDto> UpdateHakkimdaAsync(UpdateHakkimdaDto dto, int? profilResmiId = null);
    }
}
