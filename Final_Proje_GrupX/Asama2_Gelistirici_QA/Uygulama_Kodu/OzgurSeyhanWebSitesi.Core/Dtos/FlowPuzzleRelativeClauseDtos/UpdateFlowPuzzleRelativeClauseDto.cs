using System.ComponentModel.DataAnnotations;

namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleRelativeClauseDtos
{
    public class UpdateFlowPuzzleRelativeClauseDto
    {
        [Required]
        [MaxLength(800)]
        public string SourcePairEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string RelativeClauseEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string TurkishTranslation { get; set; } = string.Empty;

        [MaxLength(800)]
        public string? SourcePairTr { get; set; }

        [MaxLength(500)]
        public string? RelativeClauseTr { get; set; }

        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
