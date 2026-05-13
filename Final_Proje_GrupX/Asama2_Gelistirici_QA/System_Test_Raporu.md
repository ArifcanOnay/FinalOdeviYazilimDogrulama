# System Test Report - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Date:** May 13, 2026
**Scope:** Uçtan uca fonksiyonel doğrulama + seçili non-functional testler
**Reference:** SRS (REQ-01..REQ-04), User Stories, TCS, RTM

## 1. Fonksiyonel Testler (Functional System Tests)
Aşağıdaki testler, sistemin ana kullanıcı akışlarını uçtan uca doğrular.

| Test ID | İlgili Requirement / Story | Kapsam | Beklenen Sonuç | Sonuç |
| :--- | :--- | :--- | :--- | :--- |
| ST-01 | REQ-01 / User Story 1 | Google ile kayıt/giriş (SSO) | Başarılı login + ana sayfa yönlendirme | Passed |
| ST-02 | REQ-02 / User Story 3 | YouTube site içi oynatma | İçerik site içinde başlar, sayfadan çıkılmaz | Passed |
| ST-03 | REQ-02 / User Story 3 | Spotify site içi oynatma | İçerik site içinde başlar, sayfadan çıkılmaz | Passed |
| ST-04 | REQ-03 / User Story 2 | Doğru zincir: anlık doğrulama + skor | Yeşil kilit + skor artışı + kayıt | Passed |
| ST-05 | REQ-03 / User Story 2 | Yanlış zincir: anlık hata geri bildirimi | Kırmızı kopma + skor yok + kayıt yok | Passed |
| ST-06 | REQ-04 / Use Case 2 | Eğitmen bilgi alanı görüntüleme | Foto/biyografi/sertifika doğru görünür | Passed |

### ST-04: Oyun Modülü Uçtan Uca Doğrulama (Doğru Zincir)
- **Ön Koşullar:** Kullanıcı login; "Tense Zinciri" ekranı açık; seed veriler mevcut.
- **Adımlar:** "Present Perfect" → "I have played" → "Ben oynadım (ve etkisi sürüyor)" zinciri tamamlanır.
- **Beklenen:** Zincir tamamlandığı anda doğrulama otomatik tetiklenir; çizgiler yeşil olur, kilitlenir; skor artışı görünür; skor kalıcı olarak kaydedilir.
- **Sonuç:** Passed.

## 2. Non-Functional Testler
Bu bölüm, fonksiyonel akışların yanında en az bir kalite özelliğini doğrular.

### 2.1 Kullanılabilirlik Testi (Usability) - Anlık Geri Bildirim
- **İlgili Gereksinim:** NFR  Zincirleme oyununda hatalı bağlantı kırmızı, doğru bağlantı yeşil ve kilitlenmiş olmalı.
- **Testin Amacı:** Kullanıcının submit butonuna basmadan doğru/yanlış geri bildirimi hemen aldığını doğrulamak.
- **Adımlar:**
  1. Oyun ekranında yanlış bağlantı denenir.
  2. Ardından doğru zincir tamamlanır.
- **Beklenen:**
  - Yanlış bağlantıda çizgi kırmızıya dönüp kopar.
  - Doğru zincirde çizgiler yeşile döner ve kilitlenir.
- **Sonuç:** Passed.

### 2.2 Güvenlik Testi (Security) - Yetkisiz Erişim Kontrolü
- **İlgili Gereksinim:** REQ-01 (Login), sistem bütünlüğü.
- **Testin Amacı:** Kimliği doğrulanmamış kullanıcının korumalı sayfalara erişemediğini doğrulamak.
- **Adımlar:**
  1. Tarayıcıda oturum kapalı iken oyun sayfası URL’si doğrudan açılmaya çalışılır.
  2. Benzer şekilde skor kaydı yapan endpoint’e kimliksiz istek gönderilmeye çalışılır.
- **Beklenen:**
  - UI seviyesinde login sayfasına yönlendirme veya 401/403.
  - API seviyesinde 401/403; veri değişikliği gerçekleşmez.
- **Sonuç:** Passed.

## 3. Özet (Test Summary)
- Sistem testleri, SRS’deki **REQ-01..REQ-04** fonksiyonlarının uçtan uca çalıştığını göstermiştir.
- Entegrasyon noktaları (Google SSO, medya embed, oyun doğrulama ve DB kayıtları) beklenen şekilde çalışmıştır.
- Non-functional kapsamda en az bir kalite metriği (kullanılabilirlik) ve bir güvenlik kontrolü doğrulanmıştır.
