using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleNounDtos
{
    public class CreateFlowPuzzleNounDto
    {
        [Required(ErrorMessage = "Tekil form zorunludur")]
        public string SingularForm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Çoğul form zorunludur")]
        public string PluralForm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Türkçe anlamı zorunludur")]
        public string TurkishMeaning { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
