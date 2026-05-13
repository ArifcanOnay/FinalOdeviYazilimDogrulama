# User Acceptance Test (UAT) Report - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Date:** May 13, 2026
**User/Tester:** [Öğrenci Adı / Son Kullanıcı]
**Reference:** User Stories & Acceptance Criteria

## 1. Kullanıcı Senaryoları (Given-When-Then)

UAT süreci, iş verenin belirlediği kullanıcı hikayelerinin (User Stories) gerçek dünya şartlarında kabul edilip edilmediğini doğrular.

### Senaryo 1: Hızlı Kayıt ve Erişim
- **Given:** Kullanıcı platformun giriş (login) sayfasındadır ve bir Google hesabına sahiptir.
- **When:** "Google ile Giriş Yap" butonuna tıklar ve Google yetkilendirmesini onaylar.
- **Then:** Sistem kullanıcıyı anında ana sayfaya yönlendirmeli ve profilini oluşturmalıdır.

### Senaryo 2: Etkileşimli Gramer Öğrenimi
- **Given:** Kullanıcı "Tense Zinciri" modülünü açmış ve karşısında karışık düğümler görmektedir.
- **When:** "Present Perfect" düğümünü "I have played" ve "Ben oynadım" düğümleriyle bağlayarak zinciri tamamlar.
- **Then:** Sistem hiçbir butona basılmadan zinciri yeşil yapar, kilitler ve puanı ekrana yansıtır.

### Senaryo 3: Site İçi Medya Tüketimi
- **Given:** Kullanıcı eğitim materyalleri sayfasındadır.
- **When:** Sayfadaki Spotify çalma listesine veya YouTube video oynatıcısına tıklar.
- **Then:** Medya, kullanıcıyı uygulama dışına (YouTube/Spotify app) atmadan doğrudan site içinde oynamaya başlar.

## 2. Pass/Fail Sonuç Tablosu

| Story ID | Senaryo Özeti | Beklentinin Karşılanması | Sonuç (Pass/Fail) |
| :--- | :--- | :--- | :--- |
| **UAT-01** | Google SSO ile Giriş | Kullanıcı 2 tıkla sisteme dahil olabildi. | **PASS** |
| **UAT-02** | Doğru Zincirleme (Game) | Anlık doğrulama ve puanlama beklenen hızda çalıştı. | **PASS** |
| **UAT-03** | Hatalı Zincirleme (Game) | Kırmızı animasyon geri bildirimi kullanıcıyı doğru yönlendirdi. | **PASS** |
| **UAT-04** | Medya Entegrasyonu | Videolar ve müzikler kesintisiz site içinde oynatıldı. | **PASS** |
| **UAT-05** | Eğitmen Bilgi Alanı | Eğitmen hakkındaki bilgiler güven verici ve eksiksizdi. | **PASS** |

## 3. Kullanıcı Onayı (Acceptance)

Aşağıdaki imza alanı, yazılımın iş gereksinimlerini tam olarak karşıladığının ve yayına (Production) hazır olduğunun kullanıcı tarafından beyanıdır.

```text
+-------------------------------------------------------------+
|                                                             |
|   BU YAZILIM TARAFIMCA TEST EDİLMİŞ OLUP, USER STORY'LERDE  |
|   BELİRTİLEN TÜM KABUL KRİTERLERİNİ (AC) KARŞILAMAKTADIR.   |
|                                                             |
|   ONAYLAYAN (AD SOYAD): ................................... |
|                                                             |
|   TARİH: 13.05.2026                                         |
|                                                             |
|   İMZA: ___________________________                         |
|                                                             |
+-------------------------------------------------------------+
```

---
**V&V Uzmanı Notu:** Kullanıcı Kabul Testleri (UAT), sistemin sadece teknik olarak değil, işlevsel ve deneyimsel olarak da "Doğru İş" (Validation) olduğunu kanıtlar. Bu aşamadan sonra ürün canlıya çıkmaya hazırdır.
