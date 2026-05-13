namespace OzgurSeyhanWebSitesi.Core.Dtos.OgretmenDtos
{
    public class UpdateOgretmenDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public int Yas { get; set; }
    }
}
