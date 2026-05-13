using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleRelativeClauseDtos;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IFlowPuzzleRelativeClauseService : IGenericService<FlowPuzzleRelativeClause>
    {
        Task<List<FlowPuzzleRelativeClauseDto>> GetActiveItemsAsync();
        Task<FlowPuzzleRelativeClauseDto> CreateAsync(CreateFlowPuzzleRelativeClauseDto dto);
        Task UpdateAsync(int id, UpdateFlowPuzzleRelativeClauseDto dto);
    }
}
