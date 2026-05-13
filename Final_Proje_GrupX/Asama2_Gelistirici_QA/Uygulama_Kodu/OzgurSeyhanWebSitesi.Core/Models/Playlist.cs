using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class Playlist : BaseEntitiy
    {
        public string PlaylistId { get; set; } = string.Empty; // YouTube Playlist ID
        public string Baslik { get; set; } = string.Empty;
        public string? Aciklama { get; set; }
        public string? KategoriBaslik { get; set; } // Playlist üstünde gösterilecek kategori başlığı
        public VideoKategorisi Kategori { get; set; } = VideoKategorisi.IngilizceniGelistirecekDersler; // Hangi bölümde gösterilecek
        public int OgretmenId { get; set; }

        // Navigation Properties
        public Ogretmen Ogretmen { get; set; } = null!;
    }
}
