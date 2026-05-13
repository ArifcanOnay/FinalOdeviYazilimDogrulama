using OzgurSeyhanWebSitesi.Core.Dtos.VideoProgressDtos;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IVideoProgressService : IGenericService<VideoProgressDto>
    {
        Task<VideoProgressDto> GetProgressAsync(int userId, string videoId);
        Task<List<VideoProgressDto>> GetUserPlaylistProgressAsync(int userId, int playlistId);
        Task<List<VideoProgressDto>> GetAllUserProgressAsync(int userId);
        Task<VideoProgressDto> UpdateProgressAsync(UpdateVideoProgressDto updateDto);
        Task<Dictionary<string, decimal>> GetPlaylistProgressSummaryAsync(int userId, int playlistId);
    }
}
