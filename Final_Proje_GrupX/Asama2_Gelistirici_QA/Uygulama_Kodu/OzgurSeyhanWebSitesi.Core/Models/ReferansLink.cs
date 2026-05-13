using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class ReferansLink : BaseEntitiy
    {
        public string SiteAdi { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string? Ikon { get; set; }
        public int Sira { get; set; }
    }
}
