using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class YoutubeVideoService : GenericService<YoutubeVideo>, IYoutubeVideoService
    {
        private readonly IYoutubeApiService _youtubeApiService;
        private readonly IMapper _mapper;

        public YoutubeVideoService(
            IGenericRepository<YoutubeVideo> repository, 
            IUnitOfWorks unitOfWorks,
            IYoutubeApiService youtubeApiService,
            IMapper mapper) : base(repository, unitOfWorks)
        {
            _youtubeApiService = youtubeApiService;
            _mapper = mapper;
        }

        public async Task<YoutubeVideoDto> CreateFromYouTubeUrlAsync(string youtubeUrl, int ogretmenId, VideoKategorisi kategori = VideoKategorisi.YoutubeVideolarim, string? kategoriBaslik = null, int sira = 0)
        {
            try
            {
                // 1. YouTube API'den video bilgilerini çek
                var videoInfo = await _youtubeApiService.GetVideoInfoAsync(youtubeUrl);

                // 2. Eğer sıra 0 ise (default), o kategorideki en büyük sırayı bul ve +1 yap
                if (sira == 0)
                {
                    var existingVideos = GetAll().Where(v => v.Kategori == kategori).ToList();
                    if (existingVideos.Any())
                    {
                        sira = existingVideos.Max(v => v.Sira) + 1;
                    }
                }

                // 3. YoutubeVideo entity'si oluştur
                var youtubeVideo = new YoutubeVideo
                {
                    Baslik = videoInfo.Baslik,
                    Url = videoInfo.Url,
                    VideoId = videoInfo.VideoId,
                    KategoriBaslik = kategoriBaslik,
                    Kategori = kategori,
                    Sira = sira,
                    OgretmenId = ogretmenId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                // 4. Veritabanına kaydet
                await _repository.AddAsync(youtubeVideo);
                await _unitOfWorks.CommitAsync();

                // 5. DTO'ya map et ve döndür
                return _mapper.Map<YoutubeVideoDto>(youtubeVideo);
            }
            catch (Exception ex)
            {
                throw new Exception($"YouTube'dan video kaydedilemedi: {ex.Message}", ex);
            }
        }

        public async Task<List<YoutubeVideoDto>> CreateFromPlaylistAsync(string playlistUrl, int ogretmenId, VideoKategorisi kategori = VideoKategorisi.YoutubeVideolarim, string? kategoriBaslik = null)
        {
            try
            {
                // 1. YouTube API'den playlist'teki tüm videoları çek
                var playlistVideos = await _youtubeApiService.GetPlaylistVideosAsync(playlistUrl);

                var savedVideos = new List<YoutubeVideoDto>();

                // 2. O kategorideki en büyük sırayı bul
                var existingVideos = GetAll().Where(v => v.Kategori == kategori).ToList();
                int currentMaxSira = existingVideos.Any() ? existingVideos.Max(v => v.Sira) : -1;

                // 3. Her video için entity oluştur ve kaydet
                foreach (var videoInfo in playlistVideos)
                {
                    currentMaxSira++; // Her video için sırayı artır
                    
                    var youtubeVideo = new YoutubeVideo
                    {
                        Baslik = videoInfo.Baslik,
                        Url = videoInfo.Url,
                        VideoId = videoInfo.VideoId,
                        KategoriBaslik = kategoriBaslik,
                        Kategori = kategori,
                        Sira = currentMaxSira,
                        OgretmenId = ogretmenId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    await _repository.AddAsync(youtubeVideo);
                    savedVideos.Add(_mapper.Map<YoutubeVideoDto>(youtubeVideo));
                }

                // 3. Tüm değişiklikleri tek seferde kaydet
                await _unitOfWorks.CommitAsync();

                return savedVideos;
            }
            catch (Exception ex)
            {
                throw new Exception($"YouTube Playlist'ten videolar kaydedilemedi: {ex.Message}", ex);
            }
        }
        
        public async Task<List<YoutubeVideoDto>> GetByKategoriAsync(VideoKategorisi kategori)
        {
            var allVideos = GetAll();
            var videos = allVideos.Where(x => x.Kategori == kategori).ToList();
            return _mapper.Map<List<YoutubeVideoDto>>(videos);
        }
        
        public async Task<YoutubeVideo?> GetByVideoIdAsync(string videoId)
        {
            var allVideos = GetAll();
            return await Task.FromResult(allVideos.FirstOrDefault(x => x.VideoId == videoId));
        }
    }
}
