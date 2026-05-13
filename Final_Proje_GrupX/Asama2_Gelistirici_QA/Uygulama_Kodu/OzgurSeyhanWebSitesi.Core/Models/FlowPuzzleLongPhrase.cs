namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class FlowPuzzleLongPhrase : BaseEntitiy
    {
        // Example: article
        public string HeadNounEn { get; set; } = string.Empty;

        // Example: of the journalist
        public string OfPhraseEn { get; set; } = string.Empty;

        // Example: about inflation
        public string DetailPhraseEn { get; set; } = string.Empty;

        // Example: in the newspaper
        public string ContextPhraseEn { get; set; } = string.Empty;

        // Example: the article of the journalist about inflation in the newspaper
        public string FullPhraseEn { get; set; } = string.Empty;

        public string TurkishTranslation { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
