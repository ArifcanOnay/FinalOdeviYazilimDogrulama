using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IFlowPuzzleRelativeClauseRepository : IGenericRepository<FlowPuzzleRelativeClause>
    {
        Task<List<FlowPuzzleRelativeClause>> GetActiveItemsAsync();
    }
}
