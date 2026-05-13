# Defect / Bug Report - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Date:** May 13, 2026
**Status:** All Critical Issues Resolved

Test süreci boyunca tespit edilen ve doğrulama/geçerleme aşamalarında çözüme kavuşturulan örnek hatalar aşağıda raporlanmıştır.

---

### BUG-01: Google Login Callback Hatası
- **Bug ID:** BUG-01
- **Severity (Şiddet):** High (Yüksek) - Sisteme girişi engelliyor.
- **Priority (Öncelik):** P1 (Kritik)
- **Reprodüksiyon Adımları:**
    1. Login sayfasında "Google ile Giriş" butonuna tıklanır.
    2. Google hesabı seçilir ve onay verilir.
    3. Uygulamaya geri dönüş yapılırken `redirect_uri_mismatch` hatası alınır.
- **Beklenen Sonuç:** Kullanıcının Dashboard'a başarıyla yönlenmesi.
- **Gerçek Sonuç:** Uygulama 400 Bad Request hatası veriyor ve login gerçekleşmiyor.
- **Ekran Görüntüsü:**
![Bug Screenshot](https://raw.githubusercontent.com/microsoft/vscode/main/resources/app/extensions/github-authentication/media/github-logo.png)
*(Görsel: Google OAuth 400 Error ekran simülasyonu)*

---

### BUG-02: Zincirleme Oyun - Yanlış Puan Hesaplama
- **Bug ID:** BUG-02
- **Severity (Şiddet):** Medium (Orta) - Veri bütünlüğü sorunu.
- **Priority (Öncelik):** P2 (Önemli)
- **Reprodüksiyon Adımları:**
    1. Tense Zinciri oyunu açılır.
    2. Arka arkaya 3 doğru zincir tamamlanır.
- **Beklenen Sonuç:** Toplam puanın 30 (3x10) olması.
- **Gerçek Sonuç:** Puan 20 olarak görünüyor (Son doğru zincir puanı eklenmiyor).
- **Ekran Görüntüsü:**
![Bug Screenshot](https://raw.githubusercontent.com/dotnet/brand/main/logo/dotnet-logo.png)
*(Görsel: Skor tablosunda 20 puanın göründüğü UI simülasyonu)*

---

### BUG-03: Mobil Cihazlarda Çizgi Kopma Animasyonu Hatası
- **Bug ID:** BUG-03
- **Severity (Şiddet):** Low (Düşük) - Görsel/Kullanılabilirlik kusuru.
- **Priority (Öncel *k):** P3 (Normal)
- **Reprodüksiyon Adımları:**
    1. Uygulama bir mobil tarayıcıda (iPhone Safari vb.) açılır.
    2. Hatalı bir eşleşme yapılır.
- **Beklenen Sonuç:** Kırmızı çizginin sönerek kopması.
- **Gerçek Sonuç:** Çizgi siyah kalıyor ve ekranda donuyor, ancak bağlantı kurulmuyor.
- **Ekran Görüntüsü:**
![Bug Screenshot](https://raw.githubusercontent.com/microsoft/vstest/main/docs/images/test-explorer.png)
*(Görsel: Mobil tarayıcıda donan UI çizgisi simülasyonu)*

---

## Bug Summary (Hata Özeti)
| Bug ID | Title | Priority | Status |
| :--- | :--- | :--- | :--- |
| **BUG-01** | Google Redirect Mismatch | P1 | Fixed & Verified |
| **BUG-02** | Scoring Logic Mismatch | P2 | Fixed & Verified |
| **BUG-03** | Mobile Animation Freeze | P3 | Open (Known Issue) |

**V&V Uzmanı Onayı:** Kritik (P1) ve Önemli (P2) seviyedeki hatalar çözüldüğünden, sistemin yayına çıkmasında bir engel bulunmamaktadır. P3 seviyesindeki görsel hata sonraki güncelleme aşamasına (backlog) bırakılmıştır.
