using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class Podcast:BaseEntitiy
    {
      
        public string Baslik {  get; set; }
        public string PodcastUrl {  get; set; }
        public string? KapakResmi { get; set; }  // Podcast kapak fotoğrafı yolu
        public int Sira { get; set; } = 0;
        public int OgretmenId { get; set; }
        public Ogretmen Ogretmen { get; set; }

    }
}
