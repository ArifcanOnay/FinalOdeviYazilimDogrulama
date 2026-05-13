# Integration Test Report - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Date:** May 13, 2026
**Scope:** Modüller arası veri alışverişi (Google SSO, YouTube/Spotify, Game Module, SQL)
**Tools:** Postman (API), Browser DevTools (Network), Selenium (UI Smoke)

## 1. Modüller Arası Veri Alışverişi (Integration Points)
Bu rapor aşağıdaki entegrasyon noktalarını kapsar:
- **Auth  Google SSO (REQ-01):** Google OAuth  Uygulama oturum (session/JWT)  UI yönlendirme.
- **Medya  YouTube/Spotify (REQ-02):** UI  embed/player  üçüncü taraf içerik servisleri.
- **Oyun  Backend  SQL (REQ-03):** UI zincir eventi  doğrulama endpoint/servis  veri erişimi  skor kaydı.
- **Eğitmen Alanı  SQL (REQ-04):** UI  içerik endpoint/servis  eğitmen verisi çekme.

## 2. Test Senaryoları ve Sonuçları
Aşağıdaki senaryolar, RTM/TCS ile uyumlu olacak şekilde seçilmiştir.

| Scenario ID | İlgili Requirement | Senaryo Özeti | Beklenen Sonuç | Sonuç |
| :--- | :--- | :--- | :--- | :--- |
| IT-01 | REQ-01 / User Story 1 | Google ile giriş akışı (redirect + callback) | Kullanıcı ana sayfaya yönlenir, oturum açılır | Passed |
| IT-02 | REQ-02 / User Story 3 | YouTube içeriği site içinde oynatma | Video iframe/player üzerinde başlar, sayfadan çıkmaz | Passed |
| IT-03 | REQ-02 / User Story 3 | Spotify içeriği site içinde oynatma | Müzik embed/player üzerinde başlar, sayfadan çıkmaz | Passed |
| IT-04 | REQ-03 / User Story 2 | Zincir tamamlanınca anlık doğrulama + skor kaydı | Doğru zincir yeşil/kilit, skor DB’ye yazılır | Passed |
| IT-05 | REQ-03 / User Story 2 | Hatalı zincirde anında geri bildirim | Kırmızı kopma, skor artmaz, DB değişmez | Passed |
| IT-06 | REQ-04 / Use Case 2 | Eğitmen bilgileri dinamik yüklenir | Foto/biyografi/sertifika eksiksiz görünür | Passed |

### IT-01: Google SSO Entegrasyon Testi (REQ-01)
- **Ön Koşullar:** Google OAuth yapılandırması aktif, uygulama erişim izinleri tanımlı.
- **Adımlar:**
  1. Login sayfasında "Google ile Giriş" seçilir.
  2. Google Consent ekranı onaylanır.
  3. Callback sonrası uygulamaya dönülür.
- **Beklenen:** Kullanıcı ana sayfaya yönlenir, kimlik bilgisi oturumda tutulur.
- **Sonuç:** Passed.

### IT-04: Oyun Doğrulama + Skor Kaydı (REQ-03)
- **Ön Koşullar:** Oyun modülü açık, seed gramer verisi mevcut.
- **Adımlar:**
  1. Kullanıcı üçlü zinciri (Tense + English + Turkish) tamamlar.
  2. UI doğrulama isteğini backend’e gönderir.
  3. Backend doğrulamayı yapar ve skor günceller.
- **Beklenen:** UI yeşil/kilit + skor artışı; DB’de ilgili kullanıcı skoru güncellenir.
- **Sonuç:** Passed.

### IT-05: Oyun Hata Akışı (REQ-03)
- **Ön Koşullar:** Oyun modülü açık.
- **Adımlar:**
  1. Kullanıcı yanlış düğüme bağlanır.
  2. UI anında hata geri bildirimi verir.
- **Beklenen:** Kırmızı kopma animasyonu; skor artışı ve DB güncellemesi olmaz.
- **Sonuç:** Passed.

## 3. Notlar (V&V)
- Bu rapor, **modüller arası çalışabilirliği** (interoperability) doğrular; detaylı UI davranışı için TCS senaryoları referans alınır.
- Üçüncü taraf servislerde (Google/YouTube/Spotify) kesinti/kota durumları için STP’deki risk önlemleri geçerlidir.
