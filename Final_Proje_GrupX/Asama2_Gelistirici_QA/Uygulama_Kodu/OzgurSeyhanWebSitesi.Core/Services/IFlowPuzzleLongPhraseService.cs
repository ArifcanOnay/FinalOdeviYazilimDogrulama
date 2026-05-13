using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleLongPhraseDtos;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IFlowPuzzleLongPhraseService : IGenericService<FlowPuzzleLongPhrase>
    {
        Task<List<FlowPuzzleLongPhraseDto>> GetActiveItemsAsync();
        Task<FlowPuzzleLongPhraseDto> CreateAsync(CreateFlowPuzzleLongPhraseDto dto);
        Task UpdateAsync(int id, UpdateFlowPuzzleLongPhraseDto dto);
    }
}
