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
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Playlist?> GetByPlaylistIdAsync(string playlistId)
        {
            return await _context.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId);
        }

        public async Task<IEnumerable<Playlist>> GetByOgretmenIdAsync(int ogretmenId)
        {
            return await _context.Playlists
                .Where(p => p.OgretmenId == ogretmenId)
                .OrderByDescending(p => p.CreateDate)
                .ToListAsync();
        }
    }
}
