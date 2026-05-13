using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleNounDtos
{
    public class FlowPuzzleNounDto
    {
        public int Id { get; set; }
        public string SingularForm { get; set; } = string.Empty;
        public string PluralForm { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
