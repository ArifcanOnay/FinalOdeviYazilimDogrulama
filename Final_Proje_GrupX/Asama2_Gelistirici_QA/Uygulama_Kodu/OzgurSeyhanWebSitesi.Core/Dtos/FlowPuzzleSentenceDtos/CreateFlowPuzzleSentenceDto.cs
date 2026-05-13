using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleSentenceDtos
{
    public class CreateFlowPuzzleSentenceDto
    {
        public string Preposition { get; set; } = string.Empty;
        public string DeterminerSingular { get; set; } = string.Empty;
        public string DeterminerPlural { get; set; } = string.Empty;
        public int NounId { get; set; }
        public string TurkishSingular { get; set; } = string.Empty;
        public string TurkishPlural { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
