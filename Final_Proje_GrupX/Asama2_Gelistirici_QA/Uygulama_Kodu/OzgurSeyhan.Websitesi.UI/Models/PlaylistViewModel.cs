using System.ComponentModel.DataAnnotations;

namespace OzgurSeyhan.Websitesi.UI.Models
{
    public class CreatePlaylistViewModel
    {
        [Required(ErrorMessage = "YouTube Playlist URL'si gereklidir")]
        [Display(Name = "YouTube Playlist URL")]
        public string PlaylistUrl { get; set; } = string.Empty;

        [Display(Name = "Kategori Başlığı")]
        [MaxLength(200, ErrorMessage = "Kategori başlığı en fazla 200 karakter olabilir")]
        public string? KategoriBaslik { get; set; }

        public int OgretmenId { get; set; } = 1; // Default öğretmen ID
    }
}
