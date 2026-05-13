using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IYoutubeVideoService:IGenericService<YoutubeVideo>
    {
        /// <summary>
        /// YouTube URL'inden video bilgilerini çekip veritabanına kaydeder
        /// </summary>
        Task<YoutubeVideoDto> CreateFromYouTubeUrlAsync(string youtubeUrl, int ogretmenId, VideoKategorisi kategori = VideoKategorisi.YoutubeVideolarim, string? kategoriBaslik = null, int sira = 0);

        /// <summary>
        /// YouTube Playlist URL'inden tüm videoları çekip veritabanına kaydeder
        /// </summary>
        Task<List<YoutubeVideoDto>> CreateFromPlaylistAsync(string playlistUrl, int ogretmenId, VideoKategorisi kategori = VideoKategorisi.YoutubeVideolarim, string? kategoriBaslik = null);
        
        /// <summary>
        /// Kategoriye göre videoları getirir
        /// </summary>
        Task<List<YoutubeVideoDto>> GetByKategoriAsync(VideoKategorisi kategori);
        
        /// <summary>
        /// VideoId (string) ile video entity getirir
        /// </summary>
        Task<YoutubeVideo?> GetByVideoIdAsync(string videoId);
    }
}
