using Microsoft.Extensions.Options;
using Translation.API.DTOs;
using Translation.API.Models.Configurations;
using Translation.API.Models.Responses;

namespace Translation.API.Services
{
    public class TranslationService : ITranslationService
    {
        private TranslationApiConfig TranslationConfig { get; }
        private HttpClient HttpClient { get; }
        public TranslationService(IOptions<TranslationApiConfig> config, IHttpClientFactory clientFactory)
        {
            TranslationConfig = config.Value;
            HttpClient = clientFactory.CreateClient(TranslationConfig.HttpClientName);
        }
        public async Task<ServiceResponse<TranslationsDto>> GetTranslationsAsync(string text)
        {
            var serviceResponse = new ServiceResponse<TranslationsDto>();
            var baseCollection = new List<KeyValuePair<string, string>>
                {
                    new("q", text),
                    new("source", "en"),
                };
            try
            {
                var frenchText = await TranslateTextAsync(baseCollection, "fr");
                var spanishText = await TranslateTextAsync(baseCollection, "es");
                if (frenchText == null || spanishText == null)
                {
                    throw new Exception();
                }
                serviceResponse.Data = new TranslationsDto
                {
                    Input = text,
                    FrenchTranslation = frenchText,
                    SpanishTranslation = spanishText
                };
                serviceResponse.Message = "Successful translations";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Unsuccessful translations";
            }

            return serviceResponse;
        }
        private async Task<string?> TranslateTextAsync(List<KeyValuePair<string, string>> baseCollection, string targetLanguage)
        {
            var languageCollection = new List<KeyValuePair<string, string>>(baseCollection)
            {
                new("target", targetLanguage)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "language/translate/v2")
            {
                Content = new FormUrlEncodedContent(languageCollection)
            };

            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadFromJsonAsync<TranslationResponse>();
            return responseContent?.Data?.Translations?.FirstOrDefault()?.TranslatedText;
        }
    }
}
