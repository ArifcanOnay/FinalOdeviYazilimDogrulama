namespace OzgurSeyhanWebSitesi.Core.Dtos.OgretmenDtos
{
    public class CreateOgretmenDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public int Yas { get; set; }
    }
}
