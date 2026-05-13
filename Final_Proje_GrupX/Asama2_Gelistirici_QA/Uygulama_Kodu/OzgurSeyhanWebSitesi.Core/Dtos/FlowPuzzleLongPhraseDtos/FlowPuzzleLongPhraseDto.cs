namespace OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleLongPhraseDtos
{
    public class FlowPuzzleLongPhraseDto
    {
        public int Id { get; set; }
        public string HeadNounEn { get; set; } = string.Empty;
        public string OfPhraseEn { get; set; } = string.Empty;
        public string DetailPhraseEn { get; set; } = string.Empty;
        public string ContextPhraseEn { get; set; } = string.Empty;
        public string FullPhraseEn { get; set; } = string.Empty;
        public string TurkishTranslation { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
