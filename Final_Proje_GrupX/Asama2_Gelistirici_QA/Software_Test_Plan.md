# Software Test Plan (STP) - 5 Dakikada İngilizce
**Standard:** IEEE 829 Compliant
**Project:** 5 Dakikada İngilizce Web Platformu
**Version:** 1.0

## 1. Test Kapsamı (Test Scope)
Bu plan, "5 Dakikada İngilizce" platformunun temel fonksiyonlarını kapsar:
- **Google SSO:** Kayıt ve giriş süreçlerinin güvenliği ve hızı (REQ-01).
- **Medya Entegrasyonu:** YouTube ve Spotify API'lerinin site içi oynatma stabilitesi (REQ-02).
- **Zincirleme Oyun Modülü:** Tense ve Relative Clause yapılarının anlık doğrulanması, animasyonlar ve puanlama sistemi (REQ-03, Use Case 1).
- **Eğitmen Bilgi Alanı:** Dinamik veri çekme ve görüntüleme (REQ-04, Use Case 2).

## 2. Test Seviyeleri (Test Levels)
- **Unit Testing (Birim Testleri):** Zincirleme eşleştirme mantığının (SQL/Backend) doğruluğu, puan hesaplama fonksiyonları.
- **Integration Testing (Entegrasyon Testleri):** Google, YouTube ve Spotify API'lerinin sistemle olan veri alışverişi.
- **System Testing (Sistem Testleri):** Tüm modüllerin (UI, DB, API) bir bütün olarak uçtan uca senaryolara (Use Case) uyumu.
- **Acceptance Testing (Kabul Testleri):** İş verenin User Story'lerindeki Acceptance Criteria'lara (AC) göre son kullanıcı onayı.

## 3. Test Ortamı (Test Environment)
- **Web Tarayıcıları:** Chrome, Firefox, Edge ve Safari (Mobil uyumluluk dahil).
- **Platform:** ASP.NET Core MVC / Web API.
- **Veritabanı:** SQL Server (Gramer verileri ve kullanıcı skorları için).
- **Araçlar:** Selenium (UI Automation), xUnit/NUnit (Unit Tests), Postman (API Tests).

## 4. Risk Analizi (Risk Analysis)
| Risk | Etki | Önlem |
| :--- | :--- | :--- |
| **API Kesintisi:** Google/Spotify API'lerine erişilememesi. | Yüksek | Hata yakalama (Error Handling) ve kullanıcıya bilgilendirme mesajları. |
| **Performans:** Zincirleme oyununda anlık doğrulamadaki gecikmeler. | Orta | WebSocket veya optimize edilmiş AJAX istekleri kullanılarak "Instant Feedback" korunacak. |
| **Veri Tutarsızlığı:** Yanlış gramer eşleşmelerinin veritabanında yer alması. | Orta | Seed verilerin (SQL scripts) uzman eğitmen tarafından manuel kontrolü. |

## 5. Roller ve Sorumluklar (Roles and Responsibilities)
- **Test Yöneticisi:** Test stratejisinin belirlenmesi ve final raporunun onaylanması.
- **Yazılım Test Mühendisi:** Test senaryolarının (STD) yazılması, manuel ve otomasyon testlerinin icrası.
- **Geliştirici (Developer):** Birim testlerinin yazılması ve bulunan hataların (Bugs) düzeltilmesi.
- **Ürün Sahibi (Business Owner):** Kabul testlerinin (UAT) onaylanması.
