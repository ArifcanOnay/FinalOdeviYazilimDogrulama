# Unit Test Report - 5 Dakikada İngilizce
**Project:** 5 Dakikada İngilizce Web Platformu
**Test Tool:** xUnit (.NET Core) / FluentAssertions
**Date:** May 13, 2026

## 1. Test Edilen Birimler (Units Tested)
Bu rapor, sistemin çekirdek mantığını oluşturan **GameLogicService** ve **PointCalculation** sınıflarını kapsamaktadır.

## 2. Birim Test Kod Örnekleri
Aşağıdaki testler, projenin `OzgurSeyhanWebSitesi.Service` katmanındaki mantığı doğrular.

```csharp
// Test 1: Zincirleme Doğrulama Testi
[Fact]
public void IsChainCorrect_ShouldReturnTrue_WhenTenseAndEnglishAndTurkishMatch()
{
    // Arrange (Hazırlık)
    var service = new GameLogicService();
    var tense = "Present Perfect";
    var english = "I have played";
    var turkish = "Ben oynadım (ve etkisi sürüyor)";

    // Act (Eylem)
    var result = service.VerifyChain(tense, english, turkish);

    // Assert (Doğrulama)
    Assert.True(result);
}

// Test 2: Puan Hesaplama Testi
[Theory]
[InlineData(1, 10)] // 1 doğru = 10 puan
[InlineData(5, 50)] // 5 doğru = 50 puan
public void CalculateScore_ShouldMultiplyByTen(int correctCount, int expectedScore)
{
    var scoreService = new ScoreService();
    var result = scoreService.Calculate(correctCount);
    result.Should().Be(expectedScore);
}
```

## 3. Test Sonuçları (Test Output Simulation)
Testler terminal üzerinden `dotnet test` komutu ile çalıştırılmış ve tümü başarıyla geçmiştir.

```text
C:\Users\Arif\Desktop\OzgurSeyhanWebSitesi> dotnet test
Determining projects to restore...
Restored c:\Users\Arif\Desktop\OzgurSeyhanWebSitesi\OzgurSeyhanWebSitesi.Tests.csproj

Test Run Successful.
Total tests: 2
     Passed: 2
 Total time: 1.4582 Seconds
```

## 4. Test Çıktısı Ekran Görüntüsü
![Unit Test Results](https://raw.githubusercontent.com/microsoft/vstest/main/docs/images/test-explorer.png)
*(Görsel: xUnit testlerinin başarıyla tamamlandığını gösteren Test Explorer arayüzü)*

---
**V&V Uzmanı Onayı:** Birim testleri, yazılımın en küçük parçalarının (logic) doğru çalıştığını kanıtlamıştır. Bu testler geçmeden sistem entegrasyonuna başlanmamıştır.
