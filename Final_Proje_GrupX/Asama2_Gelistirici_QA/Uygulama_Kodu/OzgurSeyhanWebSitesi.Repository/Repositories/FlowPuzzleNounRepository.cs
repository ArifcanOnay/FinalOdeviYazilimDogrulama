using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class FlowPuzzleNounRepository : GenericRepository<FlowPuzzleNoun>, IFlowPuzzleNounRepository
    {
        public FlowPuzzleNounRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FlowPuzzleNoun>> GetActiveNounsAsync()
        {
            return await _context.Set<FlowPuzzleNoun>()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }
    }
}
