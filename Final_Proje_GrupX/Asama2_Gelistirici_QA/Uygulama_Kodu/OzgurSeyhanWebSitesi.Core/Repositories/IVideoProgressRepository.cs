using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IVideoProgressRepository : IGenericRepository<VideoProgress>
    {
        Task<VideoProgress> GetProgressAsync(int userId, string videoId);
        Task<List<VideoProgress>> GetUserPlaylistProgressAsync(int userId, int playlistId);
        Task<List<VideoProgress>> GetAllUserProgressAsync(int userId);
        Task UpdateProgressAsync(VideoProgress progress);
    }
}
