namespace Translation.API.Models.Configurations
{
    public class TranslationApiConfig
    {
        public required string ApiBaseAddress { get; set; }
        public required string HttpClientName { get; set; }
        public required Dictionary<string,string> Headers { get; set; }
    }
}
