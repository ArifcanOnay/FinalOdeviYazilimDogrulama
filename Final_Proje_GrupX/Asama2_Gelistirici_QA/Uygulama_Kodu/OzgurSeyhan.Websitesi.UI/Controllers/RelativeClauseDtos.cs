namespace OzgurSeyhan.Websitesi.UI.Controllers
{
    public class FlowPuzzleRelativeClauseDto
    {
        public int Id { get; set; }
        public string SourcePairEn { get; set; } = string.Empty;
        public string RelativeClauseEn { get; set; } = string.Empty;
        public string TurkishTranslation { get; set; } = string.Empty;
        public string? SourcePairTr { get; set; }
        public string? RelativeClauseTr { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
