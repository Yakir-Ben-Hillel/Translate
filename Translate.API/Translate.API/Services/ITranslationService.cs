using Translation.API.DTOs;
using Translation.API.Models.Responses;

namespace Translation.API.Services
{
    public interface ITranslationService
    {
        public Task<ServiceResponse<TranslationsDto>> GetTranslationsAsync(string text);
    }
}
