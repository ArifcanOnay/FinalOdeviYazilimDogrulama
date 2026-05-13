using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class PlaylistCacheService
    {
        private static readonly Dictionary<string, CachedPlaylistVideos> _cache = new();
        private static readonly object _lock = new();
        private readonly int _cacheDurationMinutes;

        public PlaylistCacheService(int cacheDurationMinutes = 60)
        {
            _cacheDurationMinutes = cacheDurationMinutes;
        }

        public List<YoutubeVideoDto>? GetCachedVideos(string playlistId)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(playlistId, out var cached))
                {
                    // Cache geçerli mi kontrol et
                    if ((DateTime.Now - cached.CachedAt).TotalMinutes < _cacheDurationMinutes)
                    {
                        return cached.Videos;
                    }
                    else
                    {
                        // Cache süresi dolmuş, sil
                        _cache.Remove(playlistId);
                    }
                }
                return null;
            }
        }

        public void SetCachedVideos(string playlistId, List<YoutubeVideoDto> videos)
        {
            lock (_lock)
            {
                _cache[playlistId] = new CachedPlaylistVideos
                {
                    Videos = videos,
                    CachedAt = DateTime.Now
                };
            }
        }

        public void ClearCache(string playlistId)
        {
            lock (_lock)
            {
                _cache.Remove(playlistId);
            }
        }

        public void ClearAllCache()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }

        private class CachedPlaylistVideos
        {
            public List<YoutubeVideoDto> Videos { get; set; } = new();
            public DateTime CachedAt { get; set; }
        }
    }
}
