namespace OzgurSeyhanWebSitesi.Core.Dtos.OzelDersDtos
{
    public class UpdateOzelDersDto
    {
        public int Id { get; set; }
        public string KurSeviyesi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public int HaftalikSaat { get; set; }
        public int MaksimumOgrenciSayisi { get; set; }
        public string Gunler { get; set; } = string.Empty;
        public string SaatAraligi { get; set; } = string.Empty;
        public string? ResimUrl { get; set; }
        public string? YoutubeVideoUrl { get; set; }
        public int Sira { get; set; } = 0;
        public int OgretmenId { get; set; }
    }
}
