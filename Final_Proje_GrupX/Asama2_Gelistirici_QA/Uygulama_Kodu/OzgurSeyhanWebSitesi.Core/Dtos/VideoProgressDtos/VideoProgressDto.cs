using System;

namespace OzgurSeyhanWebSitesi.Core.Dtos.VideoProgressDtos
{
    public class VideoProgressDto : BaseDto
    {
        public int UserId { get; set; }
        public int PlaylistId { get; set; }
        public string VideoId { get; set; }
        public decimal IzlenmeYuzdesi { get; set; }
        public int IzlenenSaniye { get; set; }
        public int ToplamSure { get; set; }
        public bool TamamlandiMi { get; set; }
        public DateTime IlkIzlemeTarihi { get; set; }
        public DateTime SonIzlemeTarihi { get; set; }
    }
}
