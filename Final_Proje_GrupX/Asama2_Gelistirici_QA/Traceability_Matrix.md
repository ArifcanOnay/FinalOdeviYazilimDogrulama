# Requirements Traceability Matrix (RTM) - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Consistency Check:** SRS, User Stories and TCS Alignment

Bu matris, iş verenin gereksinimlerinin (SRS), kullanıcı hikayelerinin (User Story) ve hazırlanan test vakalarının (Test Case) birbirini tam olarak karşıladığını doğrular.

| Requirement ID | User Story ID | Test Case ID | Testin Amacı / Kapsamı | Durum |
| :--- | :--- | :--- | :--- | :--- |
| **REQ-01** | User Story 1 | **TC-01** | Google SSO ile hızlı ve güvenli giriş/kayıt süreci. | Tested |
| **REQ-02** | User Story 3 | **TC-04** | YouTube ve Spotify medyasının site içi oynatılması. | Tested |
| **REQ-03** | User Story 2 | **TC-02** | Zincirleme oyun modülü: Doğru eşleşme ve anlık puanlama. | Tested |
| **REQ-03** | User Story 2 | **TC-03** | Zincirleme oyun modülü: Hatalı eşleşme ve animasyon/geri bildirim. | Tested |
| **REQ-04** | Use Case 2 | **TC-05** | Eğitmen biyografi, tecrübe ve sertifikalarının dinamik gösterimi. | Tested |

### Matris Analizi ve Tutarlılık Notu:
- **Tam Kapsam:** İş verenin SRS dökümanında belirttiği tüm fonksiyonel gereksinimler (REQ-01'den REQ-04'e kadar) en az bir Test Case ile eşleştirilmiştir.
- **Çift Yönlü Kontrol:** Her Test Case bir User Story'ye, her User Story de bir ana gereksinime (Requirement) dayanmaktadır.
- **Hata Yönetimi:** REQ-03 için hem başarı (TC-02) hem de hata (TC-03) durumları izlenebilirlik kapsamına alınarak test derinliği sağlanmıştır.
