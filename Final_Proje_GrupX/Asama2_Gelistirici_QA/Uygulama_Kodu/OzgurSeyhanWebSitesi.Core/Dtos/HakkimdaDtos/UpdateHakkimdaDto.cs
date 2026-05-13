namespace OzgurSeyhanWebSitesi.Core.Dtos.HakkimdaDtos
{
    public class UpdateHakkimdaDto
    {
        public int Id { get; set; }
        
        // Ana Bilgiler
        public string AdSoyad { get; set; } = null!;
        public string Unvan { get; set; } = null!;
        public string Aciklama { get; set; } = null!;
        public string? ProfilResmiUrl { get; set; }
    }
}
