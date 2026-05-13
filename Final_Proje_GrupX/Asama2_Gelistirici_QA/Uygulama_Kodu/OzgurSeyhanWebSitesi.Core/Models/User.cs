using System;
using System.Collections.Generic;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class User : BaseEntitiy
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public bool AktifMi { get; set; }
        public bool IsAdmin { get; set; } // Admin yetkisi
        public bool EmailDogrulandiMi { get; set; }
        public string? EmailDogrulamaToken { get; set; }
        public DateTime? EmailDogrulamaTokenExpiry { get; set; }
        public DateTime? EmailDogrulamaTarihi { get; set; }
        
        // Navigation Properties
        public ICollection<VideoProgress> VideoProgresses { get; set; }
    }
}
