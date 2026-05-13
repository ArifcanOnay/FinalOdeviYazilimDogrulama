using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace OzgurSeyhan.Websitesi.UI.Controllers
{
    public class FlowPuzzleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public FlowPuzzleController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7101/api";
        }

        // GET: FlowPuzzle
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var nouns = JsonSerializer.Deserialize<List<FlowPuzzleNounDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(nouns ?? new List<FlowPuzzleNounDto>());
            }

            return View(new List<FlowPuzzleNounDto>());
        }

        // GET: FlowPuzzle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlowPuzzle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFlowPuzzleNounDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDto);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var json = JsonSerializer.Serialize(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleNoun", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "İsim çifti oluşturulurken bir hata oluştu.");
            return View(createDto);
        }

        // GET: FlowPuzzle/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var noun = JsonSerializer.Deserialize<FlowPuzzleNounDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (noun != null)
                {
                    var updateDto = new UpdateFlowPuzzleNounDto
                    {
                        SingularForm = noun.SingularForm,
                        PluralForm = noun.PluralForm,
                        TurkishMeaning = noun.TurkishMeaning,
                        IsActive = noun.IsActive,
                        DisplayOrder = noun.DisplayOrder
                    };
                    ViewBag.Id = noun.Id;
                    return View(updateDto);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: FlowPuzzle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateFlowPuzzleNounDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updateDto);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleNoun/update/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "İsim çifti güncellenirken bir hata oluştu.");
            ViewBag.Id = id;
            return View(updateDto);
        }

        // POST: FlowPuzzle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleNoun/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "İsim çifti silinirken bir hata oluştu.";
            return RedirectToAction(nameof(Index));
        }

        // ==================== SENTENCE MANAGEMENT ====================

        // GET: FlowPuzzle/SentenceIndex
        public async Task<IActionResult> SentenceIndex()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Get all sentences
            var sentencesResponse = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleSentence");
            var sentences = new List<FlowPuzzleSentenceDto>();

            if (sentencesResponse.IsSuccessStatusCode)
            {
                var content = await sentencesResponse.Content.ReadAsStringAsync();
                sentences = JsonSerializer.Deserialize<List<FlowPuzzleSentenceDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<FlowPuzzleSentenceDto>();
            }

            return View(sentences);
        }

        // GET: FlowPuzzle/SentenceCreate
        public async Task<IActionResult> SentenceCreate()
        {
            // Get nouns for dropdown
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var nouns = JsonSerializer.Deserialize<List<FlowPuzzleNounDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Nouns = nouns ?? new List<FlowPuzzleNounDto>();
            }
            else
            {
                ViewBag.Nouns = new List<FlowPuzzleNounDto>();
            }

            return View();
        }

        // POST: FlowPuzzle/SentenceCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SentenceCreate(CreateFlowPuzzleSentenceDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var client2 = _httpClientFactory.CreateClient("DefaultClient");
                var response2 = await client2.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");
                if (response2.IsSuccessStatusCode)
                {
                    var content2 = await response2.Content.ReadAsStringAsync();
                    var nouns2 = JsonSerializer.Deserialize<List<FlowPuzzleNounDto>>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.Nouns = nouns2 ?? new List<FlowPuzzleNounDto>();
                }
                return View(createDto);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var json = JsonSerializer.Serialize(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleSentence", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SentenceIndex));
            }

            ModelState.AddModelError(string.Empty, "Cümle oluşturulurken bir hata oluştu.");
            return View(createDto);
        }

        // GET: FlowPuzzle/SentenceEdit/5
        public async Task<IActionResult> SentenceEdit(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // Get sentence
            var sentenceResponse = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleSentence/{id}");

            // Get nouns for dropdown
            var nounsResponse = await client.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");

            if (sentenceResponse.IsSuccessStatusCode && nounsResponse.IsSuccessStatusCode)
            {
                var sentenceContent = await sentenceResponse.Content.ReadAsStringAsync();
                var sentence = JsonSerializer.Deserialize<FlowPuzzleSentenceDto>(sentenceContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var nounsContent = await nounsResponse.Content.ReadAsStringAsync();
                var nouns = JsonSerializer.Deserialize<List<FlowPuzzleNounDto>>(nounsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Nouns = nouns ?? new List<FlowPuzzleNounDto>();

                if (sentence != null)
                {
                    var updateDto = new UpdateFlowPuzzleSentenceDto
                    {
                        Preposition = sentence.Preposition,
                        DeterminerSingular = sentence.DeterminerSingular,
                        DeterminerPlural = sentence.DeterminerPlural,
                        NounId = sentence.NounId,
                        TurkishSingular = sentence.TurkishSingular,
                        TurkishPlural = sentence.TurkishPlural,
                        IsActive = sentence.IsActive,
                        DisplayOrder = sentence.DisplayOrder
                    };
                    ViewBag.Id = sentence.Id;
                    return View(updateDto);
                }
            }

            return RedirectToAction(nameof(SentenceIndex));
        }

        // POST: FlowPuzzle/SentenceEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SentenceEdit(int id, UpdateFlowPuzzleSentenceDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                var client2 = _httpClientFactory.CreateClient("DefaultClient");
                var nounsResponse = await client2.GetAsync($"{_apiBaseUrl}/FlowPuzzleNoun");
                if (nounsResponse.IsSuccessStatusCode)
                {
                    var nounsContent = await nounsResponse.Content.ReadAsStringAsync();
                    var nouns = JsonSerializer.Deserialize<List<FlowPuzzleNounDto>>(nounsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.Nouns = nouns ?? new List<FlowPuzzleNounDto>();
                }
                return View(updateDto);
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleSentence/update/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SentenceIndex));
            }

            ModelState.AddModelError(string.Empty, "Cümle güncellenirken bir hata oluştu.");
            ViewBag.Id = id;
            return View(updateDto);
        }

        // POST: FlowPuzzle/SentenceDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SentenceDelete(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleSentence/delete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SentenceIndex));
            }

            TempData["Error"] = "Cümle silinirken bir hata oluştu.";
            return RedirectToAction(nameof(SentenceIndex));
        }

        // GET: FlowPuzzle/LoadSeedData
        public async Task<IActionResult> LoadSeedData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("DefaultClient");
                var response = await client.PostAsync($"{_apiBaseUrl}/FlowPuzzleSentence/SeedData", null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    TempData["Success"] = "100 isim ve 1200 cümle kombinasyonu başarıyla yüklendi!";
                    return RedirectToAction(nameof(SentenceIndex));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Veri yükleme hatası: {error}";
                    return RedirectToAction(nameof(SentenceIndex));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Veri yükleme hatası: {ex.Message}";
                return RedirectToAction(nameof(SentenceIndex));
            }
        }
    }

    // DTOs for UI layer
    public class FlowPuzzleNounDto
    {
        public int Id { get; set; }
        public string SingularForm { get; set; } = string.Empty;
        public string PluralForm { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateFlowPuzzleNounDto
    {
        public string SingularForm { get; set; } = string.Empty;
        public string PluralForm { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateFlowPuzzleNounDto
    {
        public string SingularForm { get; set; } = string.Empty;
        public string PluralForm { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class FlowPuzzleSentenceDto
    {
        public int Id { get; set; }
        public string Preposition { get; set; } = string.Empty;
        public string DeterminerSingular { get; set; } = string.Empty;
        public string DeterminerPlural { get; set; } = string.Empty;
        public int NounId { get; set; }
        public string NounSingular { get; set; } = string.Empty;
        public string NounPlural { get; set; } = string.Empty;
        public string TurkishSingular { get; set; } = string.Empty;
        public string TurkishPlural { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateFlowPuzzleSentenceDto
    {
        public string Preposition { get; set; } = string.Empty;
        public string DeterminerSingular { get; set; } = string.Empty;
        public string DeterminerPlural { get; set; } = string.Empty;
        public int NounId { get; set; }
        public string TurkishSingular { get; set; } = string.Empty;
        public string TurkishPlural { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateFlowPuzzleSentenceDto
    {
        public string Preposition { get; set; } = string.Empty;
        public string DeterminerSingular { get; set; } = string.Empty;
        public string DeterminerPlural { get; set; } = string.Empty;
        public int NounId { get; set; }
        public string TurkishSingular { get; set; } = string.Empty;
        public string TurkishPlural { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
