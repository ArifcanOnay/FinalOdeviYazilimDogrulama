namespace OzgurSeyhanWebSitesi.Core.Models
{
    public class FlowPuzzleRelativeClause : BaseEntitiy
    {
        // Example: "The man is my teacher. He is talking to you"
        public string SourcePairEn { get; set; } = string.Empty;

        // Example: "The man who is talking to you is my teacher."
        public string RelativeClauseEn { get; set; } = string.Empty;

        // Main Turkish translation used in the third area.
        public string TurkishTranslation { get; set; } = string.Empty;

        // Optional Turkish texts for future three-column Turkish mode.
        public string? SourcePairTr { get; set; }
        public string? RelativeClauseTr { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
