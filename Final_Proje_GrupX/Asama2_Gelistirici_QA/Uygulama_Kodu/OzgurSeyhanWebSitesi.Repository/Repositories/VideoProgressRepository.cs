using Microsoft.EntityFrameworkCore;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;

namespace OzgurSeyhanWebSitesi.Repository.Repositories
{
    public class VideoProgressRepository : GenericRepository<VideoProgress>, IVideoProgressRepository
    {
        public VideoProgressRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<VideoProgress> GetProgressAsync(int userId, string videoId)
        {
            return await _context.VideoProgresses
                .FirstOrDefaultAsync(vp => vp.UserId == userId && vp.VideoId == videoId);
        }

        public async Task<List<VideoProgress>> GetUserPlaylistProgressAsync(int userId, int playlistId)
        {
            return await _context.VideoProgresses
                .Where(vp => vp.UserId == userId && vp.PlaylistId == playlistId)
                .OrderBy(vp => vp.IlkIzlemeTarihi)
                .ToListAsync();
        }

        public async Task<List<VideoProgress>> GetAllUserProgressAsync(int userId)
        {
            return await _context.VideoProgresses
                .Where(vp => vp.UserId == userId)
                .Include(vp => vp.Playlist)
                .OrderByDescending(vp => vp.SonIzlemeTarihi)
                .ToListAsync();
        }

        public async Task UpdateProgressAsync(VideoProgress progress)
        {
            _context.VideoProgresses.Update(progress);
            await _context.SaveChangesAsync();
        }
    }
}
