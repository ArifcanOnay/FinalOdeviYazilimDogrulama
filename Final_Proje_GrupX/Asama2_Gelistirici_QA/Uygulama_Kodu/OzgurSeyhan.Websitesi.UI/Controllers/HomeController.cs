using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using OzgurSeyhan.Websitesi.UI.Models;
using OzgurSeyhan.Websitesi.UI.Filters;
using OzgurSeyhanWebSitesi.Core.Dtos.PlaylistDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.OzelDersDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.PodcastDtos;
using OzgurSeyhanWebSitesi.Core.Dtos;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace OzgurSeyhan.Websitesi.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7101/api";
        }

        public async Task<IActionResult> Index()
        {
            // Google ile login kontrol et
            if (User.Identity?.IsAuthenticated == true && !HttpContext.Session.Keys.Contains("UserId"))
            {
                var googleEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                var googleName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var googleId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrWhiteSpace(googleEmail))
                {
                    try
                    {
                        Console.WriteLine($"[GOOGLE] Email: {googleEmail}, Name: {googleName}, ID: {googleId}");

                        var googleClient = _httpClientFactory.CreateClient("DefaultClient");
                        var googleLoginData = new
                        {
                            email = googleEmail,
                            googleId = googleId,
                            name = googleName ?? googleEmail.Split('@')[0]
                        };

                        var response = await googleClient.PostAsJsonAsync($"{_apiBaseUrl}/User/google-login", googleLoginData);
                        var resultJson = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[GOOGLE] API Response: {resultJson}");

                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var apiResponse = JsonSerializer.Deserialize<LoginApiResponse>(resultJson, options);

                        if (response.IsSuccessStatusCode && apiResponse?.Data != null)
                        {
                            var user = apiResponse.Data;
                            Console.WriteLine($"[GOOGLE] ✅ Login başarılı! UserId={user.Id}, Email={user.Email}");

                            HttpContext.Session.SetInt32("UserId", user.Id);
                            HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                            HttpContext.Session.SetString("UserName", $"{user.Ad} {user.Soyad}");
                            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[GOOGLE] ❌ Hata: {ex.GetType().Name} - {ex.Message}");
                    }
                }
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var allPlaylistsWithVideos = new List<PlaylistWithVideosDto>();
            var ozelDersler = new List<OzelDersDto>();
            var ingilizceniGelistirecekPodcastler = new List<YoutubeVideoDto>();
            var ingilizceniGelistirecekYoutubeKanallari = new List<YoutubeVideoDto>();
            var ingilizceniGelistirecekDersler = new List<YoutubeVideoDto>();
            var ingilizceniGelistirecekTuyolar = new List<YoutubeVideoDto>();
            var gercekHayattanIngilizce = new List<YoutubeVideoDto>();
            var podcasts = new List<PodcastDto>();
            var referansLinkler = new List<ReferansLinkDto>();

            try
            {
                // Tüm playlists'leri al
                var playlistResponse = await client.GetAsync($"{_apiBaseUrl}/Playlist");
                if (playlistResponse.IsSuccessStatusCode)
                {
                    var allPlaylists = await playlistResponse.Content.ReadFromJsonAsync<List<PlaylistDto>>();
                    
                    if (allPlaylists != null && allPlaylists.Any())
                    {
                        // Her playlist için videoları çek
                        foreach (var playlist in allPlaylists)
                        {
                            try
                            {
                                _logger.LogInformation($"🔄 Playlist {playlist.Id} ({playlist.Baslik}) yükleniyor...");
                                var withVideosResponse = await client.GetAsync($"{_apiBaseUrl}/Playlist/{playlist.Id}/with-videos");
                                
                                if (withVideosResponse.IsSuccessStatusCode)
                                {
                                    var playlistWithVideos = await withVideosResponse.Content.ReadFromJsonAsync<PlaylistWithVideosDto>();
                                    if (playlistWithVideos != null)
                                    {
                                        _logger.LogInformation($"✅ Playlist {playlist.Id} başarıyla yüklendi - {playlistWithVideos.Videos?.Count ?? 0} video");
                                        allPlaylistsWithVideos.Add(playlistWithVideos);
                                    }
                                }
                                else
                                {
                                    var errorContent = await withVideosResponse.Content.ReadAsStringAsync();
                                    _logger.LogError($"❌ Playlist {playlist.Id} yüklenemedi - Status: {withVideosResponse.StatusCode}, Error: {errorContent}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"❌ Playlist {playlist.Id} videoları yüklenirken EXCEPTION: {ex.Message}");
                            }
                        }
                    }
                }

                // İngilizceni Geliştirecek Podcastler (Kategori 1)
                var podcastlerResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/by-kategori/1");
                if (podcastlerResponse.IsSuccessStatusCode)
                {
                    var podcastler = await podcastlerResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                    if (podcastler != null)
                    {
                        ingilizceniGelistirecekPodcastler = podcastler;
                    }
                }

                // İngilizceni Geliştirecek YouTube Kanalları (Kategori 2)
                var youtubeKanallariResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/by-kategori/2");
                if (youtubeKanallariResponse.IsSuccessStatusCode)
                {
                    var youtubeKanallari = await youtubeKanallariResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                    if (youtubeKanallari != null)
                    {
                        ingilizceniGelistirecekYoutubeKanallari = youtubeKanallari;
                    }
                }

                // Gerçek Hayattan İngilizce videolarını al (Kategori 3)
                var gercekHayatResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/by-kategori/3");
                if (gercekHayatResponse.IsSuccessStatusCode)
                {
                    var gercekHayat = await gercekHayatResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                    if (gercekHayat != null)
                    {
                        gercekHayattanIngilizce = gercekHayat;
                    }
                }

                // İngilizceni Geliştirecek Dersler (Kategori 4)
                var derslerResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/by-kategori/4");
                if (derslerResponse.IsSuccessStatusCode)
                {
                    var dersler = await derslerResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                    if (dersler != null)
                    {
                        ingilizceniGelistirecekDersler = dersler;
                    }
                }

                // İngilizceni Geliştirecek Tüyolar (Kategori 5)
                var tuyolarResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo/by-kategori/5");
                if (tuyolarResponse.IsSuccessStatusCode)
                {
                    var tuyolar = await tuyolarResponse.Content.ReadFromJsonAsync<List<YoutubeVideoDto>>();
                    if (tuyolar != null)
                    {
                        ingilizceniGelistirecekTuyolar = tuyolar;
                    }
                }

                // Özel Dersleri al
                var ozelDersResponse = await client.GetAsync($"{_apiBaseUrl}/OzelDers");
                if (ozelDersResponse.IsSuccessStatusCode)
                {
                    var ozelDersResult = await ozelDersResponse.Content.ReadFromJsonAsync<List<OzelDersDto>>();
                    if (ozelDersResult != null)
                    {
                        ozelDersler = ozelDersResult;
                    }
                }

                // Referans Linkleri al
                var referansLinkResponse = await client.GetAsync($"{_apiBaseUrl}/ReferansLink");
                if (referansLinkResponse.IsSuccessStatusCode)
                {
                    var linkler = await referansLinkResponse.Content.ReadFromJsonAsync<List<ReferansLinkDto>>();
                    if (linkler != null)
                    {
                        referansLinkler = linkler;
                    }
                }

                // Podcast'leri al
                var podcastResponse = await client.GetAsync($"{_apiBaseUrl}/Podcast");
                if (podcastResponse.IsSuccessStatusCode)
                {
                    var podcastResult = await podcastResponse.Content.ReadFromJsonAsync<List<PodcastDto>>();
                    if (podcastResult != null)
                    {
                        podcasts = podcastResult;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data yüklenirken hata oluştu");
            }

            // Hakkımda bilgilerini al
            try
            {
                var hakkimdaResponse = await client.GetAsync($"{_apiBaseUrl}/Hakkimda");
                if (hakkimdaResponse.IsSuccessStatusCode)
                {
                    var hakkimda = await hakkimdaResponse.Content.ReadFromJsonAsync<OzgurSeyhanWebSitesi.Core.Dtos.HakkimdaDtos.HakkimdaDto>();
                    ViewBag.Hakkimda = hakkimda;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hakkımda bilgileri yüklenirken hata oluştu");
            }

            ViewBag.OzelDersler = ozelDersler.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.IngilizceniGelistirecekPodcastler = ingilizceniGelistirecekPodcastler.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.IngilizceniGelistirecekYoutubeKanallari = ingilizceniGelistirecekYoutubeKanallari.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.IngilizceniGelistirecekDersler = ingilizceniGelistirecekDersler.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.IngilizceniGelistirecekTuyolar = ingilizceniGelistirecekTuyolar.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.GercekHayattanIngilizce = gercekHayattanIngilizce.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.Podcasts = podcasts.OrderBy(x => x.Sira).ThenBy(x => x.Id).ToList();
            ViewBag.ReferansLinkler = referansLinkler;
            
            // API URL'ini ViewBag'e ekle (JavaScript için)
            ViewBag.ApiUrl = _configuration["ConnectionStrings:ApiUrl"] ?? "https://localhost:7101";
            
            // Playlist'leri kategorilere göre ayır
            ViewBag.PodcastlerPlaylists = allPlaylistsWithVideos.Where(p => (int)p.Kategori == 1).ToList();
            ViewBag.YoutubeKanallariPlaylists = allPlaylistsWithVideos.Where(p => (int)p.Kategori == 2).ToList();
            ViewBag.GercekHayatPlaylists = allPlaylistsWithVideos.Where(p => (int)p.Kategori == 3).ToList();
            ViewBag.DerslerPlaylists = allPlaylistsWithVideos.Where(p => (int)p.Kategori == 4).ToList();
            ViewBag.TuyolarPlaylists = allPlaylistsWithVideos.Where(p => (int)p.Kategori == 5).ToList();
            
            // JavaScript için tüm playlists'i Model olarak gönder
            return View(allPlaylistsWithVideos);
        }

        public async Task<IActionResult> Dashboard()
        {
            Console.WriteLine("========== DASHBOARD ERİŞİMİ ==========");
            
            // Session'dan UserId al
            var userId = HttpContext.Session.GetInt32("UserId");
            var isAdminSession = HttpContext.Session.GetString("IsAdmin");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            
            Console.WriteLine($"[DASHBOARD] UserId={userId}");
            Console.WriteLine($"[DASHBOARD] UserEmail={userEmail}");
            Console.WriteLine($"[DASHBOARD] IsAdmin={isAdminSession}");
            Console.WriteLine($"[DASHBOARD] IsAdmin=='true'? {isAdminSession == "true"}");
            
            // Admin kontrolü AKTİF
            if (userId == null || isAdminSession != "true")
            {
                Console.WriteLine("[DASHBOARD] ❌ Yetkisiz erişim! Login'e yönlendiriliyor...");
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok. Lütfen admin hesabıyla giriş yapın.";
                return RedirectToAction("Login");
            }
            
            Console.WriteLine("[DASHBOARD] ✅ Admin erişimi onaylandı!");
            
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // İstatistikleri çek
            var videoCount = 0;
            var playlistCount = 0;
            var ozelDersCount = 0;
            var podcastCount = 0;

            try
            {
                // YouTube Videoları
                var videoResponse = await client.GetAsync($"{_apiBaseUrl}/YoutubeVideo");
                if (videoResponse.IsSuccessStatusCode)
                {
                    var videos = await videoResponse.Content.ReadFromJsonAsync<List<object>>();
                    videoCount = videos?.Count ?? 0;
                }

                // Playlist'ler
                var playlistResponse = await client.GetAsync($"{_apiBaseUrl}/Playlist");
                if (playlistResponse.IsSuccessStatusCode)
                {
                    var playlists = await playlistResponse.Content.ReadFromJsonAsync<List<object>>();
                    playlistCount = playlists?.Count ?? 0;
                }

                // Özel Dersler
                var ozelDersResponse = await client.GetAsync($"{_apiBaseUrl}/OzelDers");
                if (ozelDersResponse.IsSuccessStatusCode)
                {
                    var ozelDersler = await ozelDersResponse.Content.ReadFromJsonAsync<List<object>>();
                    ozelDersCount = ozelDersler?.Count ?? 0;
                }

                // Podcast'ler
                var podcastResponse = await client.GetAsync($"{_apiBaseUrl}/Podcast");
                if (podcastResponse.IsSuccessStatusCode)
                {
                    var podcasts = await podcastResponse.Content.ReadFromJsonAsync<List<object>>();
                    podcastCount = podcasts?.Count ?? 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard istatistikleri yüklenirken hata oluştu");
            }

            ViewBag.VideoCount = videoCount;
            ViewBag.PlaylistCount = playlistCount;
            ViewBag.OzelDersCount = ozelDersCount;
            ViewBag.PodcastCount = podcastCount;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // Test endpoint - Claims kontrolü
        public IActionResult TestClaims()
        {
            var claims = new System.Text.StringBuilder();
            claims.AppendLine($"Authenticated: {User.Identity?.IsAuthenticated}");
            claims.AppendLine($"Name: {User.Identity?.Name}");
            claims.AppendLine("\nAll Claims:");
            foreach (var claim in User.Claims)
            {
                claims.AppendLine($"  {claim.Type}: {claim.Value}");
            }
            return Content(claims.ToString(), "text/plain");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            Console.WriteLine("========== USERDTO İLE LOGIN ==========");
            Console.WriteLine($"Email: {email}");
            
            try
            {
                var client = _httpClientFactory.CreateClient("DefaultClient");
                var loginData = new { email, password };
                var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/User/login", loginData);
                
                var resultJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[LOGIN] Response: {resultJson}");

                // Direkt class'a deserialize et
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<LoginApiResponse>(resultJson, options);
                
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = apiResponse?.Message ?? "Email veya şifre hatalı!";
                    return View();
                }
                
                if (apiResponse?.Data == null)
                {
                    Console.WriteLine("[LOGIN] ❌ Data null!");
                    TempData["Error"] = apiResponse?.Message ?? "Beklenmeyen yanıt!";
                    return View();
                }
                
                var user = apiResponse.Data;
                Console.WriteLine($"[LOGIN] ✅ User: Id={user.Id}, Email={user.Email}, IsAdmin={user.IsAdmin}");
                
                // Session kaydet
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserName", $"{user.Ad} {user.Soyad}");
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");
                
                // Admin ise Dashboard'a yönlendir
                if (user.IsAdmin)
                {
                    Console.WriteLine("[LOGIN] ✅ Admin kullanıcı! Dashboard'a yönlendiriliyor...");
                    return RedirectToAction("Dashboard");
                }
                
                Console.WriteLine("[LOGIN] ✅ OK! Anasayfaya yönlendiriliyor...");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGIN] ❌ {ex.GetType().Name}: {ex.Message}");
                TempData["Error"] = $"Hata: {ex.Message}";
                return View();
            }
        }
        
        // API Response Models
        private class LoginApiResponse
        {
            public bool Success { get; set; }
            public UserDtoModel? Data { get; set; }
            public string? Message { get; set; }
        }

        private class BasicApiResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
        }
        
        private class UserDtoModel
        {
            public int Id { get; set; }
            public string? Ad { get; set; }
            public string? Soyad { get; set; }
            public string? Email { get; set; }
            public bool IsAdmin { get; set; }
        }

        public IActionResult Register()
        {
            ViewBag.ApiUrl = _configuration["ConnectionStrings:ApiUrl"] ?? "https://localhost:7101";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string ad, string soyad, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(ad) ||
                string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Lutfen tum alanlari doldurun.";
                return View();
            }

            if (password.Length < 6)
            {
                TempData["Error"] = "Sifre en az 6 karakter olmalidir.";
                return View();
            }

            if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                TempData["Error"] = "Sifreler eslesmiyor.";
                return View();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("DefaultClient");
                var registerData = new { ad, soyad, email, password };
                var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/User/register", registerData);
                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<BasicApiResponse>(responseContent, options);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = apiResponse?.Message ?? "Kayit sirasinda bir hata olustu.";
                    return View();
                }

                TempData["Success"] = apiResponse?.Message
                    ?? "Kaydiniz alindi. E-posta adresinize gelen dogrulama baglantisini tiklayin.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email ile kayit sirasinda hata olustu.");
                TempData["Error"] = "Kayit sirasinda beklenmeyen bir hata olustu. Lutfen tekrar deneyin.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.VerificationSuccess = false;
                ViewBag.VerificationMessage = "Doğrulama bağlantısı eksik veya hatalı.";
                return View();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("DefaultClient");
                var response = await client.GetAsync($"{_apiBaseUrl}/User/verify-email?token={Uri.EscapeDataString(token)}");
                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<BasicApiResponse>(responseContent, options);

                ViewBag.VerificationSuccess = response.IsSuccessStatusCode;
                ViewBag.VerificationMessage = apiResponse?.Message
                    ?? (response.IsSuccessStatusCode
                        ? "E-posta adresiniz doğrulandı."
                        : "E-posta doğrulaması başarısız oldu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "E-posta doğrulama sırasında hata oluştu");
                ViewBag.VerificationSuccess = false;
                ViewBag.VerificationMessage = "Doğrulama sırasında beklenmeyen bir hata oluştu.";
            }

            return View();
        }

        /// <summary>
        /// Google OAuth challenge - kullanıcıyı Google'a yönlendir
        /// </summary>
        [HttpGet]
        [Route("signin-google")]
        public IActionResult SignInGoogle(string flow = "login")
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }

            var redirectUrl = Url.Action("GoogleCallback", "Home", new { flow }) ?? "/";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string flow = "login")
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                TempData["Error"] = "Google doğrulaması başarısız oldu.";
                return RedirectToAction("Login");
            }

            var googleEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var googleName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var googleId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(googleEmail))
            {
                TempData["Error"] = "Google hesabından e-posta alınamadı.";
                return RedirectToAction("Login");
            }

            try
            {
                var googleClient = _httpClientFactory.CreateClient("DefaultClient");
                var googleLoginData = new
                {
                    email = googleEmail,
                    googleId = googleId,
                    name = googleName ?? googleEmail.Split('@')[0]
                };

                var response = await googleClient.PostAsJsonAsync($"{_apiBaseUrl}/User/google-login", googleLoginData);
                var resultJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<LoginApiResponse>(resultJson, options);

                if (!response.IsSuccessStatusCode || apiResponse?.Data == null)
                {
                    TempData["Error"] = apiResponse?.Message ?? "Google ile giriş başarısız oldu.";
                    return RedirectToAction("Login");
                }

                var user = apiResponse.Data;

                if (string.Equals(flow, "register", StringComparison.OrdinalIgnoreCase))
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.Session.Clear();
                    TempData["Success"] = "Kayıt tamamlandı. Google ile giriş yapabilirsiniz.";
                    return RedirectToAction("Login");
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserName", $"{user.Ad} {user.Soyad}");
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

                if (user.IsAdmin)
                {
                    return RedirectToAction("Dashboard");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google callback sırasında hata oluştu");
                TempData["Error"] = "Google girişi sırasında beklenmeyen bir hata oluştu.";
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            _logger.LogWarning("[LOGOUT] User logged out successfully");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckSession()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            
            if (userId.HasValue && !string.IsNullOrEmpty(userEmail))
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = userId.Value,
                        email = userEmail,
                        name = userName ?? "Kullanıcı",
                        isAdmin = isAdmin == "true"
                    }
                });
            }
            
            return Json(new { success = false, message = "Oturum bulunamadı" });
        }

        // Flow Puzzle Game Sayfası - Giriş zorunlu
        [LoginRequired]
        public async Task<IActionResult> FlowPuzzleGame(int page = 1)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            try
            {
                // Get all nouns first
                var nounsResponse = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");
                if (!nounsResponse.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var allNouns = await nounsResponse.Content.ReadFromJsonAsync<List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto>>();
                if (allNouns == null || !allNouns.Any())
                {
                    return View("Error");
                }

                // Oyun başlangıcında daha çok kullanılan kişi odaklı noun'ları öne al.
                allNouns = OrderNounsForGameStart(allNouns);

                // Calculate pagination
                int totalPages = allNouns.Count;
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                var currentNoun = allNouns[page - 1];

                // Get sentences for this noun
                var sentencesResponse = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleSentence/active");
                if (sentencesResponse.IsSuccessStatusCode)
                {
                    var allSentences = await sentencesResponse.Content.ReadFromJsonAsync<List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto>>();
                    var nounSentences = allSentences?.Where(s => s.NounId == currentNoun.Id)
                        .OrderBy(s => s.DisplayOrder)
                        .ToList() ?? new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto>();

                    // Oyun ekraninda daha ileri seviye alistirma icin uzun tamlama (in/of) uret.
                    nounSentences = BuildCompoundFlowPuzzleSentences(nounSentences, currentNoun, allNouns);

                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentNoun = currentNoun;

                    return View(nounSentences);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Flow Puzzle verileri alınırken hata oluştu");
            }

            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = 1;
            ViewBag.CurrentNoun = null;
            return View(new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto>());
        }

        [LoginRequired]
        public async Task<IActionResult> RelativeClauseGame()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            try
            {
                var response = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleRelativeClause/active");
                if (!response.IsSuccessStatusCode)
                {
                    return View(new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleRelativeClauseDto>());
                }

                var items = await response.Content.ReadFromJsonAsync<List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleRelativeClauseDto>>();
                return View(items ?? new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleRelativeClauseDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Relative clause oyun verileri alinirken hata olustu");
                return View(new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleRelativeClauseDto>());
            }
        }

        [LoginRequired]
        public async Task<IActionResult> LongPhraseGame()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            try
            {
                var response = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleLongPhrase/active");
                if (!response.IsSuccessStatusCode)
                {
                    return View(new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleLongPhraseDto>());
                }

                var items = await response.Content.ReadFromJsonAsync<List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleLongPhraseDto>>();
                return View(items ?? new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleLongPhraseDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uzun tamlama oyun verileri alinirken hata olustu");
                return View(new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleLongPhraseDto>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto> BuildCompoundFlowPuzzleSentences(
            List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto> baseSentences,
            OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto currentNoun,
            List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto> allNouns)
        {
            if (baseSentences == null || !baseSentences.Any())
            {
                return new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto>();
            }

            // Orijinal akışı koru, ancak noun ile anlamsız preposition'ları ele.
            var orderedSentences = baseSentences
                .OrderBy(s => s.DisplayOrder)
                .Where(s => IsPrepositionNaturalForNoun(currentNoun.SingularForm, s.Preposition))
                .ToList();

            if (!orderedSentences.Any())
            {
                orderedSentences = baseSentences.OrderBy(s => s.DisplayOrder).ToList();
            }

            var contextPool = allNouns
                .Where(n => n.Id != currentNoun.Id)
                .OrderBy(n => n.Id)
                .ToList();

            if (!contextPool.Any())
            {
                return orderedSentences;
            }

            var preferredContextPool = GetPreferredContextPool(contextPool);
            if (!preferredContextPool.Any())
            {
                preferredContextPool = contextPool;
            }

            var result = new List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto>();

            for (int i = 0; i < orderedSentences.Count; i++)
            {
                var sentence = orderedSentences[i];

                if (!IsExtensionSuitable(currentNoun.SingularForm, sentence.Preposition))
                {
                    sentence.TurkishSingular = NormalizeTurkishText(sentence.TurkishSingular);
                    sentence.TurkishPlural = NormalizeTurkishText(sentence.TurkishPlural);
                    result.Add(sentence);
                    continue;
                }

                var connector = ChooseConnector(sentence.Preposition, currentNoun.SingularForm);
                var contextNoun = preferredContextPool[i % preferredContextPool.Count];

                var enhanced = new OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleSentenceDto
                {
                    Id = sentence.Id,
                    Preposition = sentence.Preposition,
                    DeterminerSingular = sentence.DeterminerSingular,
                    DeterminerPlural = sentence.DeterminerPlural,
                    NounId = sentence.NounId,
                    IsActive = sentence.IsActive,
                    DisplayOrder = sentence.DisplayOrder,
                    NounSingular = BuildEnglishNounPhrase(sentence.NounSingular, contextNoun.SingularForm, connector, false),
                    NounPlural = BuildEnglishNounPhrase(sentence.NounPlural, contextNoun.SingularForm, connector, true),
                    TurkishSingular = NormalizeTurkishText(AppendTurkishQualifier(sentence.TurkishSingular, contextNoun.TurkishMeaning, connector)),
                    TurkishPlural = NormalizeTurkishText(AppendTurkishQualifier(sentence.TurkishPlural, contextNoun.TurkishMeaning, connector))
                };

                result.Add(enhanced);
            }

            return result;
        }

        private List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto> GetPreferredContextPool(
            List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto> contextPool)
        {
            var preferred = new HashSet<string>
            {
                "school", "garden", "park", "city", "country", "university", "street", "building", "room"
            };

            return contextPool.Where(n => preferred.Contains(n.SingularForm.ToLowerInvariant())).ToList();
        }

        private bool IsExtensionSuitable(string headNoun, string preposition)
        {
            if (string.IsNullOrWhiteSpace(headNoun) || string.IsNullOrWhiteSpace(preposition))
            {
                return false;
            }

            var noun = headNoun.ToLowerInvariant();
            var prep = preposition.ToLowerInvariant();

            var peopleLike = new HashSet<string>
            {
                "child", "student", "teacher", "person", "man", "woman", "friend", "doctor", "nurse", "patient"
            };

            var placeLike = new HashSet<string>
            {
                "garden", "room", "school", "university", "city", "country", "park", "building", "shop", "bank", "hospital"
            };

            if (peopleLike.Contains(noun))
            {
                return prep is "for" or "about" or "because of" or "to" or "with" or "without";
            }

            if (placeLike.Contains(noun))
            {
                return prep is "to" or "from" or "with" or "because of";
            }

            return prep is "for" or "about";
        }

        private List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto> OrderNounsForGameStart(
            List<OzgurSeyhan.Websitesi.UI.Controllers.FlowPuzzleNounDto> nouns)
        {
            var priorityOrder = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["child"] = 0,
                ["student"] = 1,
                ["person"] = 2,
                ["teacher"] = 3,
                ["friend"] = 4,
                ["woman"] = 5,
                ["man"] = 6,
                ["doctor"] = 7,
                ["nurse"] = 8,
                ["patient"] = 9
            };

            return nouns
                .OrderBy(n => priorityOrder.TryGetValue(n.SingularForm ?? string.Empty, out var p) ? p : int.MaxValue)
                .ThenBy(n => n.DisplayOrder)
                .ThenBy(n => n.Id)
                .ToList();
        }

        private string ChooseConnector(string preposition, string headNoun)
        {
            var prep = preposition.ToLowerInvariant();
            var noun = headNoun.ToLowerInvariant();

            if (prep is "for" or "about" or "because of" or "without")
            {
                return "in";
            }

            if (prep is "to" or "with")
            {
                return IsLocationLikeNoun(noun) ? "of" : "in";
            }

            return "in";
        }

        private string AppendTurkishQualifier(string baseTranslation, string contextTr, string connector)
        {
            if (string.IsNullOrWhiteSpace(baseTranslation) || string.IsNullOrWhiteSpace(contextTr))
            {
                return baseTranslation;
            }

            var qualifier = connector == "of"
                ? ToGenitive(contextTr)
                : ToLocative(contextTr) + "ki";

            return InsertQualifierNaturally(baseTranslation, qualifier);
        }

        private string InsertQualifierNaturally(string baseTranslation, string qualifier)
        {
            if (string.IsNullOrWhiteSpace(baseTranslation) || string.IsNullOrWhiteSpace(qualifier))
            {
                return baseTranslation;
            }

            var normalized = baseTranslation.Trim();
            var possessivePrefixes = new[]
            {
                "benim ", "senin ", "onun ", "bizim ", "onların ", "bir ", "bu ", "şu "
            };

            foreach (var prefix in possessivePrefixes)
            {
                if (normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var tail = normalized.Substring(prefix.Length).TrimStart();
                    return prefix + qualifier + " " + tail;
                }
            }

            return qualifier + " " + normalized;
        }

        private string NormalizeTurkishText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var normalized = text;

            // "cocuk" kokunde sık gorulen hatali bicimler
            normalized = normalized.Replace("çocukından", "çocuğundan", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukinden", "çocuğundan", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukdan", "çocuktan", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukden", "çocuktan", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukda", "çocukta", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukde", "çocukta", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocuka", "çocuğa", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocuke", "çocuğa", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukun", "çocuğun", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukın", "çocuğun", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocukı", "çocuğu", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocuki", "çocuğu", StringComparison.OrdinalIgnoreCase);

            // Bozuk çift harf/ek tekrarları
            normalized = normalized.Replace("çocuğumm", "çocuğum", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("çocuğunn", "çocuğun", StringComparison.OrdinalIgnoreCase);

            // Sahiplik zamiri + cocuk kalibini SINIRLI ve guvenli bicimde duzelt
            normalized = Regex.Replace(normalized, @"\bbenim\s+çocuk\b", "benim çocuğum", RegexOptions.IgnoreCase);
            normalized = Regex.Replace(normalized, @"\bsenin\s+çocuk\b", "senin çocuğun", RegexOptions.IgnoreCase);
            normalized = Regex.Replace(normalized, @"\bonun\s+çocuk\b", "onun çocuğu", RegexOptions.IgnoreCase);
            normalized = Regex.Replace(normalized, @"\bbizim\s+çocuk\b", "bizim çocuğumuz", RegexOptions.IgnoreCase);
            normalized = Regex.Replace(normalized, @"\bonların\s+çocuk\b", "onların çocukları", RegexOptions.IgnoreCase);

            // "kişi" kokunde yapay sahiplikli biçimleri sadeleştir
            normalized = normalized.Replace("kişimle", "kişiyle", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişinle", "kişiyle", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişisiyle", "kişiyle", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişimizle", "kişiyle", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişileriyle", "kişilerle", StringComparison.OrdinalIgnoreCase);

            normalized = normalized.Replace("kişimden", "kişiden", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişinden", "kişiden", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişisinden", "kişiden", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişimizden", "kişiden", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişilerinden", "kişilerden", StringComparison.OrdinalIgnoreCase);

            normalized = normalized.Replace("kişimde", "kişide", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişinde", "kişide", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişisinde", "kişide", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişimizde", "kişide", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişilerinde", "kişilerde", StringComparison.OrdinalIgnoreCase);

            normalized = normalized.Replace("kişime", "kişiye", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişine", "kişiye", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişisine", "kişiye", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişimize", "kişiye", StringComparison.OrdinalIgnoreCase);
            normalized = normalized.Replace("kişilerine", "kişilere", StringComparison.OrdinalIgnoreCase);

            return normalized;
        }

        private bool IsPrepositionNaturalForNoun(string noun, string preposition)
        {
            if (string.IsNullOrWhiteSpace(noun) || string.IsNullOrWhiteSpace(preposition))
            {
                return false;
            }

            var n = noun.ToLowerInvariant();
            var p = preposition.ToLowerInvariant();

            if (!IsLocationLikeNoun(n) && (p == "in" || p == "on" || p == "at"))
            {
                return false;
            }

            return true;
        }

        private string BuildEnglishNounPhrase(string baseNoun, string contextNoun, string connector, bool isPlural)
        {
            var head = baseNoun;
            var tail = $"the {contextNoun}";

            if (connector == "of")
            {
                return $"{head} of {tail}";
            }

            return $"{head} in {tail}";
        }

        private string BuildTurkishHeadPhrase(string nounTr, string contextTr, string connector, bool isPlural)
        {
            var head = isPlural ? ToTurkishPlural(nounTr) : nounTr;

            if (connector == "of")
            {
                // "the garden of the school" -> "okulun bahcesi"
                return $"{ToGenitive(contextTr)} {ToThirdPersonPossessive(head)}";
            }

            // "the children in the school" -> "okuldaki cocuklar"
            return $"{ToLocative(contextTr)}ki {head}";
        }

        private string BuildTurkishSentence(string preposition, string determiner, string headPhrase)
        {
            // Yapay çevirileri engellemek için determiner'ı Türkçe tarafta zorunlu taşımıyoruz.
            var det = string.Empty;

            return preposition switch
            {
                "to" => $"{det}{ApplyDativeToPhrase(headPhrase)}",
                "in" => $"{det}{ApplyLocativeToPhrase(headPhrase)}",
                "on" => $"{det}{ApplyLocativeToPhrase(headPhrase)}",
                "at" => $"{det}{ApplyLocativeToPhrase(headPhrase)}",
                "from" => $"{det}{ApplyAblativeToPhrase(headPhrase)}",
                "for" => $"{det}{headPhrase} için",
                "with" => $"{det}{headPhrase} ile",
                "without" => $"{det}{headPhrase} olmadan",
                "after" => $"{det}{ApplyAblativeToPhrase(headPhrase)} sonra",
                "before" => $"{det}{ApplyAblativeToPhrase(headPhrase)} önce",
                "about" => $"{det}{headPhrase} hakkında",
                "because of" => $"{det}{headPhrase} yüzünden",
                _ => $"{det}{headPhrase}"
            };
        }

        private string ApplyDativeToPhrase(string phrase)
        {
            return ApplySuffixToPhrase(phrase, AddDativeSuffix);
        }

        private string ApplyLocativeToPhrase(string phrase)
        {
            return ApplySuffixToPhrase(phrase, AddLocativeSuffix);
        }

        private string ApplyAblativeToPhrase(string phrase)
        {
            return ApplySuffixToPhrase(phrase, AddAblativeSuffix);
        }

        private string ApplySuffixToPhrase(string phrase, Func<string, string> suffixFunc)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                return phrase;
            }

            var parts = phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!parts.Any())
            {
                return phrase;
            }

            var last = parts[^1];
            parts[^1] = last + suffixFunc(last);
            return string.Join(" ", parts);
        }

        private string AddDativeSuffix(string word)
        {
            var lastVowel = GetLastTurkishVowel(word);
            var endsWithVowel = IsTurkishVowel(char.ToLowerInvariant(word[^1]));
            var suffixVowel = (lastVowel == 'e' || lastVowel == 'i' || lastVowel == 'ö' || lastVowel == 'ü') ? "e" : "a";
            return endsWithVowel ? "y" + suffixVowel : suffixVowel;
        }

        private string AddLocativeSuffix(string word)
        {
            var lastVowel = GetLastTurkishVowel(word);
            var lastChar = char.ToLowerInvariant(word[^1]);
            var voiceless = "çfhkpsşt".Contains(lastChar);
            var consonant = voiceless ? "t" : "d";
            var vowel = (lastVowel == 'e' || lastVowel == 'i' || lastVowel == 'ö' || lastVowel == 'ü') ? "e" : "a";
            return consonant + vowel;
        }

        private string AddAblativeSuffix(string word)
        {
            var lastVowel = GetLastTurkishVowel(word);
            var lastChar = char.ToLowerInvariant(word[^1]);
            var voiceless = "çfhkpsşt".Contains(lastChar);
            var consonant = voiceless ? "t" : "d";
            var vowelPart = (lastVowel == 'e' || lastVowel == 'i' || lastVowel == 'ö' || lastVowel == 'ü') ? "en" : "an";
            return consonant + vowelPart;
        }

        private string ToTurkishDeterminer(string determiner)
        {
            if (string.IsNullOrWhiteSpace(determiner))
            {
                return "";
            }

            return determiner.ToLowerInvariant() switch
            {
                "a" or "an" => "bir ",
                "the" => "",
                "my" => "benim ",
                "your" => "senin ",
                "his" or "her" => "onun ",
                "our" => "bizim ",
                "their" => "onların ",
                "this" => "bu ",
                "that" => "şu ",
                "these" => "bu ",
                "those" => "şu ",
                _ => ""
            };
        }

        private string ToLocative(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            var lastVowel = GetLastTurkishVowel(word);
            var lastChar = char.ToLowerInvariant(word[^1]);
            var voiceless = "çfhkpsşt".Contains(lastChar);
            var consonant = voiceless ? 't' : 'd';
            var vowel = (lastVowel == 'e' || lastVowel == 'i' || lastVowel == 'ö' || lastVowel == 'ü') ? 'e' : 'a';

            return word + consonant + vowel;
        }

        private string ToGenitive(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            var lastVowel = GetLastTurkishVowel(word);
            var endsWithVowel = IsTurkishVowel(char.ToLowerInvariant(word[^1]));

            var suffix = lastVowel switch
            {
                'a' or 'ı' => endsWithVowel ? "nın" : "ın",
                'e' or 'i' => endsWithVowel ? "nin" : "in",
                'o' or 'u' => endsWithVowel ? "nun" : "un",
                _ => endsWithVowel ? "nün" : "ün"
            };

            return word + suffix;
        }

        private string ToThirdPersonPossessive(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            var lastVowel = GetLastTurkishVowel(word);
            var endsWithVowel = IsTurkishVowel(char.ToLowerInvariant(word[^1]));

            var core = lastVowel switch
            {
                'a' or 'ı' => "ı",
                'e' or 'i' => "i",
                'o' or 'u' => "u",
                _ => "ü"
            };

            return endsWithVowel ? word + "s" + core : word + core;
        }

        private string ToTurkishPlural(string word)
        {
            if (word.EndsWith("lar", StringComparison.OrdinalIgnoreCase) ||
                word.EndsWith("ler", StringComparison.OrdinalIgnoreCase))
            {
                return word;
            }

            var lastVowel = GetLastTurkishVowel(word);
            return lastVowel switch
            {
                'e' or 'i' or 'ö' or 'ü' => word + "ler",
                _ => word + "lar"
            };
        }

        private char GetLastTurkishVowel(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return 'a';
            }

            for (int i = word.Length - 1; i >= 0; i--)
            {
                var c = char.ToLowerInvariant(word[i]);
                if (c == 'a' || c == 'e' || c == 'ı' || c == 'i' || c == 'o' || c == 'ö' || c == 'u' || c == 'ü')
                {
                    return c;
                }
            }

            return 'a';
        }

        private bool IsTurkishVowel(char c)
        {
            return c == 'a' || c == 'e' || c == 'ı' || c == 'i' || c == 'o' || c == 'ö' || c == 'u' || c == 'ü';
        }

        private bool IsSimpleDeterminer(string determiner)
        {
            if (string.IsNullOrWhiteSpace(determiner))
            {
                return true;
            }

            var d = determiner.ToLowerInvariant();
            return d == "a" || d == "an" || d == "the";
        }

        private bool IsLocationLikeNoun(string singularForm)
        {
            if (string.IsNullOrWhiteSpace(singularForm))
            {
                return false;
            }

            var s = singularForm.ToLowerInvariant();
            var locations = new HashSet<string>
            {
                "house", "school", "university", "city", "country", "park", "building", "shop", "restaurant", "cafe",
                "hospital", "bank", "forest", "island", "room", "kitchen", "bathroom", "bedroom", "garden", "car",
                "bus", "train", "plane", "boat", "ship", "box", "bag", "bottle", "table", "chair", "door", "window",
                "street", "road", "bridge", "wall", "floor", "mountain", "river", "lake", "sea", "beach", "tree"
            };

            return locations.Contains(s);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
