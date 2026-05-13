using System;

namespace OzgurSeyhanWebSitesi.Core.Dtos.UserDtos
{
    public class UserDto : BaseDto
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public bool AktifMi { get; set; }
        public bool IsAdmin { get; set; }
        public bool EmailDogrulandiMi { get; set; }
    }
}
