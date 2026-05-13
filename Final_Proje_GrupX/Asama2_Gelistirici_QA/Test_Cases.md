# Test Case Specification (TCS) - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Reference:** User Stories & Acceptance Criteria

---

### TC-01: Google SSO Kimlik Doğrulama Testi
- **Test Case ID:** TC-01
- **Testin Amacı:** Kullanıcının Google hesabı ile sisteme sorunsuz ve hızlı giriş yapabildiğini doğrulamak (REQ-01).
- **Ön Koşullar:** Kullanıcının aktif bir Google hesabı olmalı ve sisteme daha önce giriş yapmamış (veya çıkış yapmış) olmalıdır.
- **Test Adımları:**
    1. Giriş sayfasına (Login) gidilir.
    2. "Google ile Giriş Yap" butonuna tıklanır.
    3. Google hesap bilgileri girilir ve uygulama izni onaylanır.
- **Beklenen Sonuç:** Uygulama, Google'dan gelen token'ı doğrulamalı ve kullanıcıyı ana sayfaya (Dashboard) yönlendirmelidir. Kullanıcı adı sağ üstte dökülmelidir.
- **Test Seviyesi:** System Testing / Acceptance Testing

---

### TC-02: Zincirleme Oyun Modülü - Doğru Eşleştirme (Present Perfect)
- **Test Case ID:** TC-02
- **Testin Amacı:** Üçlü zincir (Gramer kuralı + İngilizce Cümle + Türkçe Anlam) doğru tamamlandığında anlık doğrulamanın çalıştığını teyit etmek (REQ-03, Use Case 1).
- **Ön Koşullar:** Kullanıcı sisteme giriş yapmış ve "Tense Zinciri" oyun modülünü başlatmış olmalıdır.
- **Test Adımları:**
    1. "Present Perfect" düğümü seçilir.
    2. Çizgi çekilerek "I have played" düğümüne bağlanır.
    3. Zincir, "Ben oynadım (ve etkisi sürüyor)" düğümüne bağlanarak tamamlanır.
- **Beklenen Sonuç:** Zincir tamamlandığı anda çizgiler yeşile dönmeli, düğümler kilitlenmeli ve ekranda puan artışı görülmelidir.
- **Test Seviyesi:** System Testing / Integration Testing

---

### TC-03: Zincirleme Oyun Modülü - Hatalı Eşleştirme
- **Test Case ID:** TC-03
- **Testin Amacı:** Hatalı bir eşleşme yapıldığında sistemin anında negatif geri bildirim verdiğini doğrulamak.
- **Ön Koşullar:** Oyun modülü aktif olmalıdır.
- **Test Adımları:**
    1. "Present Perfect" düğümü seçilir.
    2. Çizgi çekilerek "I am playing" (Present Continuous) düğümüne bağlanmaya çalışılır.
- **Beklenen Sonuç:** Çizgi kırmızı renge dönmeli, bağlantı kurulmamalı ve yanlış düğüm eski konumuna geri dönmelidir. Puan verilmemelidir.
- **Test Seviyesi:** System Testing

---

### TC-04: Site İçi Medya Oynatma (YouTube/Spotify)
- **Test Case ID:** TC-04
- **Testin Amacı:** Harici API entegrasyonlarının site dışına çıkmadan çalıştığını doğrulamak (REQ-02, User Story 3).
- **Ön Koşullar:** Kullanıcı "Dinleme ve İzleme" sayfasında olmalıdır.
- **Test Adımları:**
    1. Sayfadaki YouTube video gömme (embed) alanındaki "Oynat" butonuna tıklanır.
    2. Spotify çalma listesi alanındaki bir şarkıya tıklanır.
- **Beklenen Sonuç:** Videonun ve müziğin tarayıcıdan ayrılmadan, site içerisindeki iframe/player üzerinde oynamaya başladığı görülmelidir.
- **Test Seviyesi:** Integration Testing

---

### TC-05: Eğitmen Bilgi Alanı Görüntüleme
- **Test Case ID:** TC-05
- **Testin Amacı:** Eğitmen bilgilerinin veritabanından dinamik ve doğru şekilde ana sayfaya yansıdığını kontrol etmek (REQ-04).
- **Ön Koşullar:** Veritabanında (SQL) eğitmen bilgileri (biyografi, sertifika vb.) yüklü olmalıdır.
- **Test Adımları:**
    1. Ana sayfa açılır ve "Eğitmenimiz" bölümüne kaydırılır.
    2. Görüntülenen bilgiler veritabanındaki kayıtlarla karşılaştırılır.
- **Beklenen Sonuç:** Fotoğraf, biyografi ve sertifikalar güncel ve düzgün formatta (CSS bozulması olmadan) görüntülenmelidir.
- **Test Seviyesi:** System Testing
