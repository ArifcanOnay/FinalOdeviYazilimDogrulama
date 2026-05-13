namespace OzgurSeyhanWebSitesi.Core.Dtos
{
    public class ReferansLinkDto
    {
        public int Id { get; set; }
        public string SiteAdi { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; }
    }
}
