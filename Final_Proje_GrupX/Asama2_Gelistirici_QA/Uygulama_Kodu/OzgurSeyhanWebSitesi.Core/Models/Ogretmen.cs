using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class Ogretmen:BaseEntitiy
    {
       
        public string Ad{  get; set; }
        public string Soyad{ get; set; }
        public int Yas {  get; set; }
        public ICollection<Podcast> Podcasts { get; set; }
        public ICollection<YoutubeVideo> YoutubeVideolari { get; set; }
        public ICollection<OzelDers>OzelDersler { get; set; }
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();



    }
}
