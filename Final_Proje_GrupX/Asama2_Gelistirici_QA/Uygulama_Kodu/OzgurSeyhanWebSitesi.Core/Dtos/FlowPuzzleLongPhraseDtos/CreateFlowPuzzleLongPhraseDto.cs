using System.ComponentModel.DataAnnotations;

namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleLongPhraseDtos
{
    public class CreateFlowPuzzleLongPhraseDto
    {
        [Required]
        [MaxLength(200)]
        public string HeadNounEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string OfPhraseEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string DetailPhraseEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string ContextPhraseEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(900)]
        public string FullPhraseEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(900)]
        public string TurkishTranslation { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
