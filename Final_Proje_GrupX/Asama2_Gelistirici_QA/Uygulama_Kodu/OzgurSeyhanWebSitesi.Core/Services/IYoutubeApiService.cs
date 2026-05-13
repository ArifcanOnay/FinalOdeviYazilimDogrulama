using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IYoutubeApiService
    {
        /// <summary>
        /// YouTube URL'inden video ID'sini çıkarır
        /// </summary>
        string ExtractVideoId(string url);

        /// <summary>
        /// YouTube API'den video bilgilerini çeker
        /// </summary>
        Task<YoutubeVideoInfoDto> GetVideoInfoAsync(string videoUrl);

        /// <summary>
        /// YouTube Playlist URL'inden playlist ID'sini çıkarır
        /// </summary>
        string ExtractPlaylistId(string playlistUrl);

        /// <summary>
        /// YouTube API'den playlist'teki tüm videoları çeker
        /// </summary>
        Task<List<YoutubeVideoInfoDto>> GetPlaylistVideosAsync(string playlistUrl);
    }

    /// <summary>
    /// YouTube API'den gelen video bilgileri için DTO
    /// </summary>
    public class YoutubeVideoInfoDto
    {
        public required string VideoId { get; set; }
        public required string Baslik { get; set; }
        public required string Url { get; set; }
    }
}
