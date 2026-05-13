using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class FlowPuzzleRelativeClauseRepository : GenericRepository<FlowPuzzleRelativeClause>, IFlowPuzzleRelativeClauseRepository
    {
        public FlowPuzzleRelativeClauseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FlowPuzzleRelativeClause>> GetActiveItemsAsync()
        {
            return await _context.Set<FlowPuzzleRelativeClause>()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }
    }
}
