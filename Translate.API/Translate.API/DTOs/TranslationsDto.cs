namespace Translation.API.DTOs
{
    public class TranslationsDto
    {
        public required string Input { get; set; }
        public required string FrenchTranslation { get; set; }
        public required string SpanishTranslation { get; set; }
    }
}
