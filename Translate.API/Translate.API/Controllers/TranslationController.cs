using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Translation.API.DTOs;
using Translation.API.Models.Responses;
using Translation.API.Services;

namespace Translate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private ITranslationService TranslationService { get; }
        public TranslationController(ITranslationService translationService)
        {
            TranslationService = translationService;
        }
        // Each Response is cached for 1h.
        [ResponseCache(Duration = 3600)]
        [HttpGet("Translate/{text}")]
        public async Task<ActionResult<ServiceResponse<TranslationsDto>>> GetTranslations(string text)
        {
            var serviceResponse = await TranslationService.GetTranslationsAsync(text);
            if (serviceResponse.Success)
                return Ok(serviceResponse);
            else
                return StatusCode(500, serviceResponse);
        }
    }
}
