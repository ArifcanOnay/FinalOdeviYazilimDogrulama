using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
   public class YoutubeVideo:BaseEntitiy
    {
       
        public string Baslik {  get; set; }
        public string Url {  get; set; }
        public string VideoId {  get; set; }
        public string? KategoriBaslik { get; set; }
        public string? Aciklama { get; set; }
        public VideoKategorisi Kategori { get; set; } = VideoKategorisi.YoutubeVideolarim;
        public int Sira { get; set; } = 0;
        public int OgretmenId { get; set; }
        public Ogretmen Ogretmen { get; set; }

    }
}
