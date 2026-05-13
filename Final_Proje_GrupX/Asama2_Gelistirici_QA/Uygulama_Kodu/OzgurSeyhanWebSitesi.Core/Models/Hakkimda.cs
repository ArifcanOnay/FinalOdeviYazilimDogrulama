namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class Hakkimda : BaseEntitiy
    {
        // Ana Bilgiler
        public string AdSoyad { get; set; } = null!;
        public string Unvan { get; set; } = null!;
        public string Aciklama { get; set; } = null!;
        public string? ProfilResmiUrl { get; set; }
    }
}
