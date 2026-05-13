using Microsoft.AspNetCore.Mvc;
using OzgurSeyhanWebSitesi.Core.Dtos.PlaylistDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.PodcastDtos;
using OzgurSeyhanWebSitesi.Core.Dtos;
using OzgurSeyhan.Websitesi.UI.Models;
using OzgurSeyhan.Websitesi.UI.Filters;
using System.Text;
using System.Text.Json;

namespace OzgurSeyhan.Websitesi.UI.Controllers
{
    [AdminAuthorize] // Admin kontrolü aktif - Sadece admin kullanıcılar erişebilir
    public class YoutubeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public YoutubeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7101/api";
        }

        #region Playlist ��lemleri

        // Playlist Listesi
        public async Task<IActionResult> PlaylistIndex()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/Playlist");

            if (response.IsSuccessStatusCode)
            {
                var playlists = await response.Content.ReadFromJsonAsync<List<PlaylistDto>>();
                return View(playlists);
            }

            return View(new List<PlaylistDto>());
        }

        // Playlist Ekleme Sayfas�
        [HttpGet]
        public IActionResult PlaylistCreate()
        {
            return View();
        }

        // Playlist Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> PlaylistCreate(CreatePlaylistViewModel model, int kategori = 4)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            
          
            
            var requestBody = new
            {
                playlistUrl = model.PlaylistUrl,
                kategoriBaslik = model.KategoriBaslik,
                kategori = kategori,
                ogretmenId = model.OgretmenId
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/Playlist/from-url", content);

            if (response.IsSuccessStatusCode)
            {
                var kategoriAdi = kategori switch
                {
                    1 => "İngilizceni Geliştirecek Podcastler",
                    2 => "İngilizceni Geliştirecek YouTube Kanalları",
                    3 => "Gerçek Hayattan İngilizce",
                    4 => "İngilizceni Geliştirecek Dersler",
                    _ => "Bilinmeyen Kategori"
                };
                TempData["SuccessMessage"] = $"Playlist '{kategoriAdi}' bölümüne başarıyla eklendi!";
                return RedirectToAction("PlaylistIndex");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Hata: {error}";
            return View(model);
        }

        // Playlist Silme
        [HttpPost]
        public async Task<IActionResult> PlaylistDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/Playlist/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Playlist ba�ar�yla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Playlist silinirken hata olu�tu!";
            }

            return RedirectToAction("PlaylistIndex");
        }

        // Playlist Videolar� G�r�nt�leme
        public async Task<IActionResult> PlaylistVideos(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/Playlist/{id}/with-videos");

            if (response.IsSuccessStatusCode)
            {
                var playlist = await response.Content.ReadFromJsonAsync<PlaylistWithVideosDto>();
                ViewBag.ApiUrl = _configuration["ConnectionStrings:ApiUrl"] ?? "https://localhost:7101";
                return View(playlist);
            }

            return RedirectToAction("PlaylistIndex");
        }

        // Playlist'e Yeni Video Ekleme
        [HttpPost]
        public async Task<IActionResult> AddNewVideosToPlaylist(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/Playlist/{id}/add-new-videos", null);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(jsonString);
                var root = jsonDoc.RootElement;
                
                var addedCount = root.GetProperty("addedCount").GetInt32();
                var message = root.GetProperty("message").GetString();

                if (addedCount > 0)
                {
                    TempData["SuccessMessage"] = message ?? $"{addedCount} yeni video ba�ar�yla eklendi!";
                }
                else
                {
                    TempData["InfoMessage"] = "Playlist zaten g�ncel. Eklenecek yeni video bulunamad�.";
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Video eklenirken hata olu�tu: {error}";
            }

            return RedirectToAction("PlaylistIndex");
        }

        #endregion

        #region Video ��lemleri

        // Video Listesi
        public async Task<IActionResult> VideoIndex()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo");

            if (response.IsSuccessStatusCode)
            {
                var videos = await response.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                ViewBag.ApiUrl = _configuration["ConnectionStrings:ApiUrl"] ?? "https://localhost:7101";
                return View(videos);
            }

            ViewBag.ApiUrl = _configuration["ConnectionStrings:ApiUrl"] ?? "https://localhost:7101";
            return View(new List<YoutubeVideoDto>());
        }

        // Video Ekleme Sayfas�
        [HttpGet]
        public IActionResult VideoCreate()
        {
            return View();
        }

        // Video Ekleme ��lemi
        [HttpPost]
        public async Task<IActionResult> VideoCreate(string videoUrl, string? kategoriBaslik, int kategori = 1, int sira = 0)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            var requestBody = new
            {
                YoutubeUrl = videoUrl,
                OgretmenId = 1,  // Sabit ��retmen ID
                Kategori = kategori,  // Kategori: 1=YoutubeVideolarim, 2=IngilizceKonusmaTuyolari
                KategoriBaslik = kategoriBaslik,  // Opsiyonel kategori ba�l���
                Sira = sira  // Sıra numarası
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/YoutubeVideo/from-url", content);

            if (response.IsSuccessStatusCode)
            {
                var kategoriAdi = kategori switch
                {
                    1 => "İngilizceni Geliştirecek Podcastler",
                    2 => "İngilizceni Geliştirecek YouTube Kanalları",
                    3 => "Gerçek Hayattan İngilizce",
                    4 => "İngilizceni Geliştirecek Dersler",
                    _ => "Bilinmeyen Kategori"
                };
                TempData["SuccessMessage"] = $"Video '{kategoriAdi}' b�l�m�ne ba�ar�yla eklendi!";
                return RedirectToAction("VideoIndex");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Hata: {error}";
            return View();
        }

        // Video G�ncelleme Sayfas�
        [HttpGet]
        public async Task<IActionResult> VideoEdit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/{id}");

            if (response.IsSuccessStatusCode)
            {
                var video = await response.Content.ReadFromJsonAsync<YoutubeVideoDto>();
                return View(video);
            }

            return RedirectToAction("VideoIndex");
        }

        // Video G�ncelleme ��lemi
        [HttpPost]
        public async Task<IActionResult> VideoEdit(YoutubeVideoDto video)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Önce mevcut videoyu al (eski sırasını ve kategorisini bilmek için)
            var getResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/{video.Id}");
            if (getResponse.IsSuccessStatusCode)
            {
                var eskiVideo = await getResponse.Content.ReadFromJsonAsync<YoutubeVideoDto>();
                
                // Eğer sıra değiştiyse, sıra kaydırma mantığını çalıştır
                if (eskiVideo != null && eskiVideo.Sira != video.Sira)
                {
                    // Aynı kategorideki tüm videoları al
                    var allVideosResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo");
                    if (allVideosResponse.IsSuccessStatusCode)
                    {
                        var allVideos = await allVideosResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                        if (allVideos != null)
                        {
                            // Yeni sırada aynı kategoride başka video var mı kontrol et
                            var cakisanVideo = allVideos.FirstOrDefault(v => 
                                v.Id != video.Id && 
                                v.Kategori == eskiVideo.Kategori && 
                                v.Sira == video.Sira);
                            
                            // Çakışan video varsa, sadece o video ile yer değiştir (SWAP)
                            if (cakisanVideo != null)
                            {
                                var swapDto = new
                                {
                                    id = cakisanVideo.Id,
                                    baslik = cakisanVideo.Baslik,
                                    url = cakisanVideo.Url,
                                    videoId = cakisanVideo.VideoId,
                                    ogretmenId = cakisanVideo.OgretmenId,
                                    sira = eskiVideo.Sira  // Eski sırayı ver
                                };
                                var swapJson = JsonSerializer.Serialize(swapDto);
                                var swapContent = new StringContent(swapJson, Encoding.UTF8, "application/json");
                                await client.PostAsync($"{_apiBaseUrl}/YoutubeVideo/update/{cakisanVideo.Id}", swapContent);
                            }
                        }
                    }
                }
            }

            var updateDto = new
            {
                id = video.Id,
                baslik = video.Baslik,
                url = video.Url,
                videoId = video.VideoId,
                ogretmenId = video.OgretmenId,
                sira = video.Sira,
                aciklama = video.Aciklama
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/YoutubeVideo/update/{video.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Video ba�ar�yla g�ncellendi!";
                return RedirectToAction("VideoIndex");
            }

            var errorDetail = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Video güncellenirken hata oluştu! (HTTP {(int)response.StatusCode}: {errorDetail})";
            return View(video);
        }

        // Video Silme
        [HttpPost]
        public async Task<IActionResult> VideoDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/YoutubeVideo/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Video başarıyla silindi!";
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Video silinirken hata oluştu! (HTTP {(int)response.StatusCode}: {errorDetail})";
            }

            return RedirectToAction("VideoIndex");
        }

        #endregion

        #region Podcast ��lemleri

        // Podcast Listesi
        public async Task<IActionResult> PodcastIndex()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/Podcast");

            if (response.IsSuccessStatusCode)
            {
                var podcasts = await response.Content.ReadFromJsonAsync<List<PodcastDto>>();
                return View(podcasts);
            }

            return View(new List<PodcastDto>());
        }

        // Podcast Ekleme Sayfas�
        [HttpGet]
        public IActionResult PodcastCreate()
        {
            return View();
        }

        // Podcast Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> PodcastCreate(string podcastUrl)
        {
            if (string.IsNullOrWhiteSpace(podcastUrl))
            {
                TempData["ErrorMessage"] = "Spotify URL boş olamaz!";
                return View();
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");

            var requestBody = new
            {
                baslik = "Spotify Playlist", // Otomatik başlık
                podcastUrl = podcastUrl,
                kapakResmi = (string?)null,
                ogretmenId = 1  // Sabit öğretmen ID
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/Podcast", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Spotify şarkısı başarıyla eklendi!";
                return RedirectToAction("PodcastIndex");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Hata: {error}";
            return View();
        }

        // Podcast G�ncelleme Sayfas�
        [HttpGet]
        public async Task<IActionResult> PodcastEdit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/Podcast/{id}");

            if (response.IsSuccessStatusCode)
            {
                var podcast = await response.Content.ReadFromJsonAsync<PodcastDto>();
                return View(podcast);
            }

            return RedirectToAction("PodcastIndex");
        }

        // Podcast G�ncelleme ��lemi
        [HttpPost]
        public async Task<IActionResult> PodcastEdit(int id, string baslik, string podcastUrl, int sira)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Önce mevcut podcast'i al (eski sırasını bilmek için)
            var getResponse = await client.GetAsync($"{_apiBaseUrl}/Podcast/{id}");
            if (getResponse.IsSuccessStatusCode)
            {
                var eskiPodcast = await getResponse.Content.ReadFromJsonAsync<PodcastDto>();
                
                // Eğer sıra değiştiyse, sıra kaydırma mantığını çalıştır
                if (eskiPodcast != null && eskiPodcast.Sira != sira)
                {
                    // Tüm podcast'leri al
                    var allPodcastsResponse = await client.GetAsync($"{_apiBaseUrl}/Podcast");
                    if (allPodcastsResponse.IsSuccessStatusCode)
                    {
                        var allPodcasts = await allPodcastsResponse.Content.ReadFromJsonAsync<List<PodcastDto>>();
                        if (allPodcasts != null)
                        {
                            // Yeni sırada başka podcast var mı kontrol et
                            var cakisanPodcast = allPodcasts.FirstOrDefault(p => 
                                p.Id != id && 
                                p.Sira == sira);
                            
                            // Çakışan podcast varsa, sadece o podcast ile yer değiştir (SWAP)
                            if (cakisanPodcast != null)
                            {
                                var swapDto = new
                                {
                                    id = cakisanPodcast.Id,
                                    baslik = cakisanPodcast.Baslik,
                                    podcastUrl = cakisanPodcast.PodcastUrl,
                                    ogretmenId = cakisanPodcast.OgretmenId,
                                    sira = eskiPodcast.Sira  // Eski sırayı ver
                                };
                                var swapJson = JsonSerializer.Serialize(swapDto);
                                var swapContent = new StringContent(swapJson, Encoding.UTF8, "application/json");
                                await client.PostAsync($"{_apiBaseUrl}/Podcast/update/{cakisanPodcast.Id}", swapContent);
                            }
                        }
                    }
                }
            }

            var updateDto = new
            {
                id = id,
                baslik = baslik,
                podcastUrl = podcastUrl,
                ogretmenId = 1,
                sira = sira
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/Podcast/update/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Podcast başarıyla güncellendi!";
                return RedirectToAction("PodcastIndex");
            }

            var podcastErrorDetail = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Podcast güncellenirken hata oluştu! (HTTP {(int)response.StatusCode}: {podcastErrorDetail})";
            return RedirectToAction("PodcastEdit", new { id });
        }

        // Podcast Silme
        [HttpPost]
        public async Task<IActionResult> PodcastDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/Podcast/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Podcast başarıyla silindi!";
            }
            else
            {
                var podcastDeleteError = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Podcast silinirken hata oluştu! (HTTP {(int)response.StatusCode}: {podcastDeleteError})";
            }

            return RedirectToAction("PodcastIndex");
        }

        #endregion

        #region ReferansLink İşlemleri

        // ReferansLink Listesi
        public async Task<IActionResult> ReferansLinkIndex()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/ReferansLink");

            if (response.IsSuccessStatusCode)
            {
                var links = await response.Content.ReadFromJsonAsync<List<ReferansLinkDto>>();
                return View(links);
            }

            return View(new List<ReferansLinkDto>());
        }

        // ReferansLink Ekleme Sayfası
        [HttpGet]
        public IActionResult ReferansLinkCreate()
        {
            return View();
        }

        // ReferansLink Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> ReferansLinkCreate(ReferansLinkDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/ReferansLink", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Link başarıyla eklendi!";
                return RedirectToAction("ReferansLinkIndex");
            }

            TempData["ErrorMessage"] = "Link eklenirken hata oluştu!";
            return View(model);
        }

        // ReferansLink Düzenleme Sayfası
        [HttpGet]
        public async Task<IActionResult> ReferansLinkEdit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/ReferansLink/{id}");

            if (response.IsSuccessStatusCode)
            {
                var link = await response.Content.ReadFromJsonAsync<ReferansLinkDto>();
                return View(link);
            }

            TempData["ErrorMessage"] = "Link bulunamadı!";
            return RedirectToAction("ReferansLinkIndex");
        }

        // ReferansLink Düzenleme İşlemi
        [HttpPost]
        public async Task<IActionResult> ReferansLinkEdit(int id, ReferansLinkDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/ReferansLink/update/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Link başarıyla güncellendi!";
                return RedirectToAction("ReferansLinkIndex");
            }

            TempData["ErrorMessage"] = "Link güncellenirken hata oluştu!";
            return View(model);
        }

        // ReferansLink Silme
        public async Task<IActionResult> ReferansLinkDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/ReferansLink/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Link başarıyla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Link silinirken hata oluştu!";
            }

            return RedirectToAction("ReferansLinkIndex");
        }

        #endregion
    }
}

