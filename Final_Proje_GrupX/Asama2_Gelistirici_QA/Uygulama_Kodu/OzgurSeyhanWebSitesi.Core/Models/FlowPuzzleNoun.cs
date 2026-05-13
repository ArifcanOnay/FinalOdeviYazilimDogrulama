using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class FlowPuzzleNoun : BaseEntitiy
    {
        public string SingularForm { get; set; } = string.Empty; // child
        public string PluralForm { get; set; } = string.Empty; // children
        public string TurkishMeaning { get; set; } = string.Empty; // çocuk
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
