using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class OzelDers:BaseEntitiy
    {
       
        public string KurSeviyesi { get; set; }
        public string Aciklama { get; set; }
        public int HaftalikSaat { get; set; }
        public int MaksimumOgrenciSayisi { get; set; }
        public string Gunler { get; set; }
        public string SaatAraligi { get; set; }
        public string? ResimUrl { get; set; }
        public string? YoutubeVideoUrl { get; set; }
        public int Sira { get; set; } = 0;
        public int OgretmenId { get; set; }
        public Ogretmen Ogretmen { get; set; }
    }
}
