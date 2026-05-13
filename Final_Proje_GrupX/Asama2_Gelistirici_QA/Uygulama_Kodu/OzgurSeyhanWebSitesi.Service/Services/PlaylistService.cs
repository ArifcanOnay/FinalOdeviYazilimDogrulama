using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.PlaylistDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class PlaylistService : GenericService<Playlist>, IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IYoutubeApiService _youtubeApiService;
        private readonly IMapper _mapper;
        private readonly PlaylistCacheService _cacheService;
        private readonly IYoutubeVideoRepository _youtubeVideoRepository;

        public PlaylistService(
            IGenericRepository<Playlist> repository,
            IUnitOfWorks unitOfWorks,
            IPlaylistRepository playlistRepository,
            IYoutubeApiService youtubeApiService,
            IMapper mapper,
            PlaylistCacheService cacheService,
            IYoutubeVideoRepository youtubeVideoRepository) : base(repository, unitOfWorks)
        {
            _playlistRepository = playlistRepository;
            _youtubeApiService = youtubeApiService;
            _mapper = mapper;
            _cacheService = cacheService;
            _youtubeVideoRepository = youtubeVideoRepository;
        }

        public async Task<PlaylistDto> CreateFromYouTubePlaylistAsync(string playlistUrl, int ogretmenId, VideoKategorisi kategori = VideoKategorisi.IngilizceniGelistirecekDersler, string? kategoriBaslik = null)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"Service - PlaylistUrl: {playlistUrl}");
                Console.WriteLine($"Service - Kategori: {kategori}");
                Console.WriteLine($"Service - KategoriBaslik: {kategoriBaslik ?? "NULL"}");
                Console.WriteLine($"Service - OgretmenId: {ogretmenId}");

                // 1. Playlist ID'sini çıkar
                var playlistId = _youtubeApiService.ExtractPlaylistId(playlistUrl);
                Console.WriteLine($"📝 Playlist ID extracted: {playlistId}");

                // 2. YouTube'dan playlist videolarını çek
                var playlistVideos = await _youtubeApiService.GetPlaylistVideosAsync(playlistUrl);
                Console.WriteLine($"📹 YouTube'dan {playlistVideos.Count} video çekildi");
                
                // 3. Videoları DTO'ya dönüştür ve cache'e kaydet
                var videoDtos = playlistVideos.Select(v => new YoutubeVideoDto
                {
                    VideoId = v.VideoId,
                    Baslik = v.Baslik,
                    Url = v.Url,
                    OgretmenId = ogretmenId
                }).ToList();

                // Cache'e kaydet (60 dakika)
                _cacheService.SetCachedVideos(playlistId, videoDtos);
                Console.WriteLine($"💾 {videoDtos.Count} video cache'e kaydedildi - Key: {playlistId}");

                // 4. Bu playlist daha önce eklenmiş mi kontrol et
                var existingPlaylist = await _playlistRepository.GetByPlaylistIdAsync(playlistId);
                if (existingPlaylist != null)
                {
                    Console.WriteLine($"ℹ️ Playlist zaten mevcut, ID: {existingPlaylist.Id}");
                    return _mapper.Map<PlaylistDto>(existingPlaylist);
                }

                // 5. İlk videoyu al
                var firstVideo = playlistVideos.FirstOrDefault();

                // 6. Playlist'i oluştur ve kaydet
                var playlist = new Playlist
                {
                    PlaylistId = playlistId,
                    Baslik = firstVideo?.Baslik ?? "YouTube Playlist",
                    Kategori = kategori, // Kategori ekle
                    KategoriBaslik = kategoriBaslik, // Kategori başlığını ekle
                    OgretmenId = ogretmenId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                Console.WriteLine($"Service - Playlist.Kategori before save: {playlist.Kategori}");
                Console.WriteLine($"Service - Playlist.KategoriBaslik before save: {playlist.KategoriBaslik ?? "NULL"}");

                await _repository.AddAsync(playlist);
                await _unitOfWorks.CommitAsync();

                Console.WriteLine($"Service - Playlist.Kategori after save: {playlist.Kategori}");
                Console.WriteLine($"Service - Playlist.KategoriBaslik after save: {playlist.KategoriBaslik ?? "NULL"}");

                return _mapper.Map<PlaylistDto>(playlist);
            }
            catch (Exception ex)
            {
                throw new Exception($"Playlist oluşturulamadı: {ex.Message}", ex);
            }
        }

        public async Task<PlaylistWithVideosDto> GetPlaylistWithVideosAsync(int playlistId)
        {
            try
            {
                // 1. Veritabanından playlist'i getir
                var playlist = await _repository.GetByIdAsync(playlistId);
                if (playlist == null)
                    throw new Exception("Playlist bulunamadı");

                // 2. CACHE KONTROLÜ - Önce cache'den bak
                var cachedVideos = _cacheService.GetCachedVideos(playlist.PlaylistId);
                
                List<YoutubeVideoDto> videos;
                
                if (cachedVideos != null && cachedVideos.Any())
                {
                    // Cache'den geldi, YouTube API kullanmadık!
                    Console.WriteLine($"✅ Cache'den {cachedVideos.Count} video bulundu - PlaylistId: {playlist.PlaylistId}");
                    videos = cachedVideos;
                }
                else
                {
                    // Cache'de yok, YouTube'dan çek
                    Console.WriteLine($"⚠️ Cache'de video yok, YouTube'dan çekiliyor - PlaylistId: {playlist.PlaylistId}");
                    var playlistUrl = $"https://www.youtube.com/playlist?list={playlist.PlaylistId}";
                    var youtubeVideos = await _youtubeApiService.GetPlaylistVideosAsync(playlistUrl);

                    // DTO'ya dönüştür
                    videos = youtubeVideos.Select(v => new YoutubeVideoDto
                    {
                        VideoId = v.VideoId,
                        Baslik = v.Baslik,
                        Url = v.Url,
                        OgretmenId = playlist.OgretmenId
                    }).ToList();

                    // Cache'e kaydet (60 dakika)
                    _cacheService.SetCachedVideos(playlist.PlaylistId, videos);
                    Console.WriteLine($"✅ {videos.Count} video cache'e kaydedildi");
                }

                // 3. DTO'ya map et
                var playlistDto = _mapper.Map<PlaylistWithVideosDto>(playlist);
                playlistDto.Videos = videos;

                // 4. DB'deki açıklamaları videolara ekle
                var dbVideos = _youtubeVideoRepository.GetAll();
                var dbVideoDict = dbVideos
                    .Where(v => !string.IsNullOrEmpty(v.Aciklama))
                    .ToDictionary(v => v.VideoId, v => v.Aciklama);

                foreach (var video in playlistDto.Videos)
                {
                    if (dbVideoDict.TryGetValue(video.VideoId, out var aciklama))
                    {
                        video.Aciklama = aciklama;
                    }
                }
                
                // İlk videonun thumbnail'ini kapak resmi olarak kullan
                if (videos.Any())
                {
                    var firstVideoId = videos.First().VideoId;
                    playlistDto.KapakResmi = $"https://img.youtube.com/vi/{firstVideoId}/maxresdefault.jpg";
                }

                return playlistDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Playlist videoları alınamadı: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<PlaylistDto>> GetByOgretmenIdAsync(int ogretmenId)
        {
            var playlists = await _playlistRepository.GetByOgretmenIdAsync(ogretmenId);
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        public async Task<int> AddNewVideosToPlaylistAsync(int playlistId)
        {
            try
            {
                // 1. Veritabanından playlist'i getir
                var playlist = await _repository.GetByIdAsync(playlistId);
                if (playlist == null)
                    throw new Exception("Playlist bulunamadı");

                Console.WriteLine($"🔄 Playlist güncelleniyor - ID: {playlistId}, YouTubePlaylistId: {playlist.PlaylistId}");

                // 2. Cache'deki mevcut videoları al
                var cachedVideos = _cacheService.GetCachedVideos(playlist.PlaylistId);
                var existingVideoIds = cachedVideos?.Select(v => v.VideoId).ToHashSet() ?? new HashSet<string>();
                
                Console.WriteLine($"📦 Cache'de {existingVideoIds.Count} mevcut video bulundu");

                // 3. YouTube'dan güncel videoları çek
                var playlistUrl = $"https://www.youtube.com/playlist?list={playlist.PlaylistId}";
                var youtubeVideos = await _youtubeApiService.GetPlaylistVideosAsync(playlistUrl);
                
                Console.WriteLine($"🎬 YouTube'dan {youtubeVideos.Count} toplam video çekildi");

                // 4. Yeni videoları filtrele (sadece cache'de olmayanlar)
                var newVideos = youtubeVideos
                    .Where(v => !existingVideoIds.Contains(v.VideoId))
                    .ToList();

                Console.WriteLine($"✨ {newVideos.Count} yeni video bulundu");

                if (newVideos.Any())
                {
                    // 5. Yeni videoları DTO'ya dönüştür
                    var newVideoDtos = newVideos.Select(v => new YoutubeVideoDto
                    {
                        VideoId = v.VideoId,
                        Baslik = v.Baslik,
                        Url = v.Url,
                        OgretmenId = playlist.OgretmenId
                    }).ToList();

                    // 6. Tüm videoları birleştir (eski + yeni)
                    var allVideos = (cachedVideos ?? new List<YoutubeVideoDto>())
                        .Concat(newVideoDtos)
                        .ToList();

                    // 7. Cache'i güncelle
                    _cacheService.SetCachedVideos(playlist.PlaylistId, allVideos);
                    
                    Console.WriteLine($"💾 Cache güncellendi - Toplam {allVideos.Count} video (Eski: {cachedVideos?.Count ?? 0}, Yeni: {newVideoDtos.Count})");

                    // 8. Playlist'in UpdateDate'ini güncelle
                    playlist.UpdateDate = DateTime.Now;
                    await _unitOfWorks.CommitAsync();

                    return newVideos.Count;
                }
                else
                {
                    Console.WriteLine("ℹ️ Eklenecek yeni video bulunamadı");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Playlist'e video eklenirken hata oluştu: {ex.Message}", ex);
            }
        }
    }
}
