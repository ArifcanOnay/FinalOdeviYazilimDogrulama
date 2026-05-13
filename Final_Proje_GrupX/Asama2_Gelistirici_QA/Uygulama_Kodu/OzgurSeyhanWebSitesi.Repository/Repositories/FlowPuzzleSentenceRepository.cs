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
    public class FlowPuzzleSentenceRepository : GenericRepository<FlowPuzzleSentence>, IFlowPuzzleSentenceRepository
    {
        public FlowPuzzleSentenceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FlowPuzzleSentence>> GetActiveSentencesWithNounsAsync()
        {
            return await _context.Set<FlowPuzzleSentence>()
                .Include(x => x.Noun)
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }
    }
}
