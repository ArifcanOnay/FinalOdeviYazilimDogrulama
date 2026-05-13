using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using OzgurSeyhanWebSitesi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class YoutubeApiService : IYoutubeApiService
    {
        private readonly YouTubeService _googleYoutubeClient;
        private readonly string _apiKey;

        public YoutubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeAPI:ApiKey"] 
                ?? throw new ArgumentNullException("YouTubeAPI:ApiKey", "YouTube API Key bulunamadı!");

            _googleYoutubeClient = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "OzgurSeyhanWebSitesi"
            });
        }

        public async Task<YoutubeVideoInfoDto> GetVideoInfoAsync(string videoUrl)
        {
            try
            {
                var videoId = ExtractVideoId(videoUrl);

                var videoRequest = _googleYoutubeClient.Videos.List("snippet");
                videoRequest.Id = videoId;

                var videoResponse = await videoRequest.ExecuteAsync();
                var video = videoResponse.Items?.FirstOrDefault();

                if (video == null)
                    throw new Exception($"Video bulunamadı: {videoId}");

                return new YoutubeVideoInfoDto
                {
                    VideoId = videoId,
                    Baslik = video.Snippet.Title,
                    Url = $"https://www.youtube.com/watch?v={videoId}"
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"YouTube'dan video bilgisi alınamadı: {ex.Message}", ex);
            }
        }

        public string ExtractVideoId(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    throw new ArgumentException("URL boş olamaz", nameof(url));

                var uri = new Uri(url);

                // youtu.be/VIDEO_ID formatı
                if (uri.Host.Contains("youtu.be"))
                {
                    return uri.AbsolutePath.Trim('/');
                }

                // youtube.com/watch?v=VIDEO_ID formatı
                if (uri.Host.Contains("youtube.com"))
                {
                    var queryString = uri.Query.TrimStart('?');
                    var queryParams = queryString.Split('&')
                        .Select(q => q.Split('='))
                        .ToDictionary(q => q[0], q => q.Length > 1 ? q[1] : string.Empty);

                    if (queryParams.TryGetValue("v", out var videoId) && !string.IsNullOrWhiteSpace(videoId))
                    {
                        return videoId;
                    }

                    throw new Exception("URL'de video ID bulunamadı");
                }

                throw new Exception("Geçersiz YouTube URL formatı");
            }
            catch (Exception ex)
            {
                throw new Exception($"Video ID çıkarılamadı: {ex.Message}", ex);
            }
        }

        public string ExtractPlaylistId(string playlistUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playlistUrl))
                    throw new ArgumentException("Playlist URL boş olamaz", nameof(playlistUrl));

                var uri = new Uri(playlistUrl);

                // youtube.com/playlist?list=PLAYLIST_ID formatı
                if (uri.Host.Contains("youtube.com"))
                {
                    var queryString = uri.Query.TrimStart('?');
                    var queryParams = queryString.Split('&')
                        .Select(q => q.Split('='))
                        .ToDictionary(q => q[0], q => q.Length > 1 ? q[1] : string.Empty);

                    if (queryParams.TryGetValue("list", out var playlistId) && !string.IsNullOrWhiteSpace(playlistId))
                    {
                        return playlistId;
                    }

                    throw new Exception("URL'de playlist ID bulunamadı");
                }

                throw new Exception("Geçersiz YouTube Playlist URL formatı");
            }
            catch (Exception ex)
            {
                throw new Exception($"Playlist ID çıkarılamadı: {ex.Message}", ex);
            }
        }

        public async Task<List<YoutubeVideoInfoDto>> GetPlaylistVideosAsync(string playlistUrl)
        {
            try
            {
                var playlistId = ExtractPlaylistId(playlistUrl);
                var videos = new List<YoutubeVideoInfoDto>();

                string? nextPageToken = null;

                do
                {
                    var playlistRequest = _googleYoutubeClient.PlaylistItems.List("snippet");
                    playlistRequest.PlaylistId = playlistId;
                    playlistRequest.MaxResults = 50; // YouTube API maksimum 50 video döner
                    playlistRequest.PageToken = nextPageToken;

                    var playlistResponse = await playlistRequest.ExecuteAsync();

                    if (playlistResponse.Items != null)
                    {
                        foreach (var item in playlistResponse.Items)
                        {
                            var videoId = item.Snippet.ResourceId.VideoId;
                            videos.Add(new YoutubeVideoInfoDto
                            {
                                VideoId = videoId,
                                Baslik = item.Snippet.Title,
                                Url = $"https://www.youtube.com/watch?v={videoId}"
                            });
                        }
                    }

                    nextPageToken = playlistResponse.NextPageToken;

                } while (!string.IsNullOrEmpty(nextPageToken));

                return videos;
            }
            catch (Exception ex)
            {
                throw new Exception($"YouTube'dan playlist bilgisi alınamadı: {ex.Message}", ex);
            }
        }
    }
}
