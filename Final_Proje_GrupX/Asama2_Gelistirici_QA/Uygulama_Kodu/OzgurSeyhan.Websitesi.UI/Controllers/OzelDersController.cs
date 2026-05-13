using Microsoft.AspNetCore.Mvc;
using OzgurSeyhanWebSitesi.Core.Dtos.OgretmenDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.OzelDersDtos;
using OzgurSeyhan.Websitesi.UI.Filters;
using System.Text;
using System.Text.Json;

namespace OzgurSeyhan.Websitesi.UI.Controllers
{
    [AdminAuthorize] // Admin kontrolü aktif - Sadece admin kullanıcılar erişebilir
    public class OzelDersController : Controller
    {
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private const long MaxImageBytes = 10 * 1024 * 1024; // 10 MB

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly string _apiBaseUrl;

        public OzelDersController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _environment = environment;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7101/api";
        }

        // Özel Ders Listesi
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/OzelDers");

            if (response.IsSuccessStatusCode)
            {
                var ozelDersler = await response.Content.ReadFromJsonAsync<List<OzelDersDto>>();
                return View(ozelDersler);
            }

            return View(new List<OzelDersDto>());
        }

        // Özel Ders Ekleme Sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Özel Ders Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> Create(CreateOzelDersDto createDto, IFormFile? ResimDosya)
        {
            // Optional fields removed from UI: supply defaults and clear validation.
            if (createDto.HaftalikSaat == 0)
            {
                createDto.HaftalikSaat = 1;
            }

            if (createDto.MaksimumOgrenciSayisi == 0)
            {
                createDto.MaksimumOgrenciSayisi = 1;
            }

            if (string.IsNullOrWhiteSpace(createDto.Gunler))
            {
                createDto.Gunler = "-";
            }

            if (string.IsNullOrWhiteSpace(createDto.SaatAraligi))
            {
                createDto.SaatAraligi = "-";
            }

            ModelState.Remove(nameof(createDto.HaftalikSaat));
            ModelState.Remove(nameof(createDto.MaksimumOgrenciSayisi));
            ModelState.Remove(nameof(createDto.Gunler));
            ModelState.Remove(nameof(createDto.SaatAraligi));

            if (!ModelState.IsValid)
            {
                return View(createDto);
            }

            bool imageUploadFailed = false;

            // Resim dosyası yüklendiyse kaydet
            if (ResimDosya != null && ResimDosya.Length > 0)
            {
                if (ResimDosya.Length > MaxImageBytes)
                {
                    ModelState.AddModelError("ResimDosya", "Resim boyutu en fazla 10 MB olabilir.");
                    return View(createDto);
                }

                var extension = Path.GetExtension(ResimDosya.FileName);
                if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ResimDosya", "Sadece .jpg, .jpeg, .png ve .webp dosyaları yüklenebilir.");
                    return View(createDto);
                }

                try
                {
                    var webRootPath = _environment.WebRootPath;
                    if (string.IsNullOrWhiteSpace(webRootPath))
                    {
                        webRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
                    }

                    var uploadsFolder = Path.Combine(webRootPath, "images", "ozelders");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // İstemciden gelen dosya adını güvenli hale getir
                    var safeFileName = Path.GetFileNameWithoutExtension(ResimDosya.FileName);
                    foreach (var invalidChar in Path.GetInvalidFileNameChars())
                    {
                        safeFileName = safeFileName.Replace(invalidChar, '_');
                    }
                    var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı kaydet
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ResimDosya.CopyToAsync(fileStream);
                    }

                    // URL'yi DTO'ya ata
                    createDto.ResimUrl = $"/images/ozelders/{uniqueFileName}";
                }
                catch (Exception ex)
                {
                    // Canlı ortamda dosya izin hatası olsa bile özel ders kaydını engelleme
                    imageUploadFailed = true;
                    createDto.ResimUrl = null;
                    TempData["ImageUploadErrorDetails"] = ex.Message;
                }
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Öğretmen seçimi UI'da yapılmıyor: sistemdeki ilk öğretmeni otomatik ata
            var defaultOgretmenId = await GetDefaultOgretmenIdAsync(client);
            if (defaultOgretmenId == null)
            {
                TempData["ErrorMessage"] = "Özel ders eklemek için önce en az bir öğretmen kaydı olmalı.";
                return View(createDto);
            }
            createDto.OgretmenId = defaultOgretmenId.Value;

            // Eğer sıra 0 ise (default), mevcut en büyük sırayı bul ve +1 yap
            if (createDto.Sira == 0)
            {
                var tempResponse = await client.GetAsync($"{_apiBaseUrl}/OzelDers");
                if (tempResponse.IsSuccessStatusCode)
                {
                    var existingDersler = await tempResponse.Content.ReadFromJsonAsync<List<OzelDersDto>>();
                    if (existingDersler != null && existingDersler.Any())
                    {
                        createDto.Sira = existingDersler.Max(d => d.Sira) + 1;
                    }
                }
            }

            var json = JsonSerializer.Serialize(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/OzelDers", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = imageUploadFailed
                    ? $"Özel ders eklendi, ancak resim yüklenemedi (sunucu dosya izinlerini kontrol edin). {(TempData["ImageUploadErrorDetails"] is string details && !string.IsNullOrWhiteSpace(details) ? "Detay: " + details : string.Empty)}"
                    : "Özel ders başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"Hata: {error}";
            return View(createDto);
        }

        // Özel Ders Güncelleme Sayfası
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/OzelDers/{id}");

            if (response.IsSuccessStatusCode)
            {
                var ozelDers = await response.Content.ReadFromJsonAsync<OzelDersDto>();
                
                if (ozelDers == null)
                {
                    return RedirectToAction("Index");
                }

                var updateDto = new UpdateOzelDersDto
                {
                    Id = ozelDers.Id,
                    KurSeviyesi = ozelDers.KurSeviyesi,
                    Aciklama = ozelDers.Aciklama,
                    HaftalikSaat = ozelDers.HaftalikSaat,
                    MaksimumOgrenciSayisi = ozelDers.MaksimumOgrenciSayisi,
                    Gunler = ozelDers.Gunler,
                    SaatAraligi = ozelDers.SaatAraligi,
                    OgretmenId = ozelDers.OgretmenId
                };

                return View(updateDto);
            }

            return RedirectToAction("Index");
        }

        // Özel Ders Güncelleme İşlemi
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateOzelDersDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Önce mevcut dersi al (eski sırasını bilmek için)
            var getResponse = await client.GetAsync($"{_apiBaseUrl}/OzelDers/{updateDto.Id}");
            if (getResponse.IsSuccessStatusCode)
            {
                var eskiDers = await getResponse.Content.ReadFromJsonAsync<OzelDersDto>();
                
                // Eğer sıra değiştiyse, sıra SWAP mantığını çalıştır
                if (eskiDers != null && eskiDers.Sira != updateDto.Sira)
                {
                    // Tüm dersleri al
                    var allDerslerResponse = await client.GetAsync($"{_apiBaseUrl}/OzelDers");
                    if (allDerslerResponse.IsSuccessStatusCode)
                    {
                        var allDersler = await allDerslerResponse.Content.ReadFromJsonAsync<List<OzelDersDto>>();
                        if (allDersler != null)
                        {
                            // Yeni sırada başka ders var mı kontrol et
                            var cakisanDers = allDersler.FirstOrDefault(d => 
                                d.Id != updateDto.Id && 
                                d.Sira == updateDto.Sira);
                            
                            // Çakışan ders varsa, sadece o ders ile yer değiştir (SWAP)
                            if (cakisanDers != null)
                            {
                                var swapDto = new UpdateOzelDersDto
                                {
                                    Id = cakisanDers.Id,
                                    KurSeviyesi = cakisanDers.KurSeviyesi,
                                    Aciklama = cakisanDers.Aciklama,
                                    HaftalikSaat = cakisanDers.HaftalikSaat,
                                    MaksimumOgrenciSayisi = cakisanDers.MaksimumOgrenciSayisi,
                                    Gunler = cakisanDers.Gunler,
                                    SaatAraligi = cakisanDers.SaatAraligi,
                                    OgretmenId = cakisanDers.OgretmenId,
                                    Sira = eskiDers.Sira  // Eski sırayı ver
                                };
                                var swapJson = JsonSerializer.Serialize(swapDto);
                                var swapContent = new StringContent(swapJson, Encoding.UTF8, "application/json");
                                await client.PostAsync($"{_apiBaseUrl}/OzelDers/update/{cakisanDers.Id}", swapContent);
                            }
                        }
                    }
                }
            }

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/OzelDers/update/{updateDto.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Özel ders başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Özel ders güncellenirken hata oluştu!";
            return View(updateDto);
        }

        // Özel Ders Silme
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/OzelDers/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Özel ders başarıyla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Özel ders silinirken hata oluştu!";
            }

            return RedirectToAction("Index");
        }

        private async Task<int?> GetDefaultOgretmenIdAsync(HttpClient client)
        {
            var response = await client.GetAsync($"{_apiBaseUrl}/Ogretmen");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var ogretmenler = await response.Content.ReadFromJsonAsync<List<OgretmenDto>>();
            return ogretmenler?.OrderBy(x => x.Id).FirstOrDefault()?.Id;
        }
    }
}
