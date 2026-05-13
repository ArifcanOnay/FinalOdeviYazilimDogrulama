using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IFlowPuzzleLongPhraseRepository : IGenericRepository<FlowPuzzleLongPhrase>
    {
        Task<List<FlowPuzzleLongPhrase>> GetActiveItemsAsync();
    }
}
