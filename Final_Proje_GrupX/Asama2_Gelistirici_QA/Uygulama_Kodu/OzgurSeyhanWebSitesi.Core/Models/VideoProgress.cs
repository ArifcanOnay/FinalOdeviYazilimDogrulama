using System;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class VideoProgress : BaseEntitiy
    {
        public int UserId { get; set; }
        public int PlaylistId { get; set; }
        public string VideoId { get; set; } // YouTube Video ID
        public decimal IzlenmeYuzdesi { get; set; } // 0-100 arası
        public int IzlenenSaniye { get; set; } // Kaç saniye izlendi
        public int ToplamSure { get; set; } // Videonun toplam süresi (saniye)
        public bool TamamlandiMi { get; set; } // %95+ ise true
        public DateTime IlkIzlemeTarihi { get; set; }
        public DateTime SonIzlemeTarihi { get; set; }
        
        // Navigation Properties
        public User User { get; set; }
        public Playlist Playlist { get; set; }
    }
}
