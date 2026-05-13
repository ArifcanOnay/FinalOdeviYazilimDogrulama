using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class FlowPuzzleLongPhraseRepository : GenericRepository<FlowPuzzleLongPhrase>, IFlowPuzzleLongPhraseRepository
    {
        public FlowPuzzleLongPhraseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FlowPuzzleLongPhrase>> GetActiveItemsAsync()
        {
            return await _context.Set<FlowPuzzleLongPhrase>()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }
    }
}
