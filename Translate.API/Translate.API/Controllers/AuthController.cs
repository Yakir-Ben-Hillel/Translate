namespace Translation.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Translation.API.Models.Responses;
    using Translation.API.Services;

    namespace Translate.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private IAuthService AuthService { get; }
            public AuthController(IAuthService authService)
            {
                AuthService = authService;
            }
            [HttpGet("HandShake")]
            public ActionResult<ServiceResponse<TokenResponse>> HandShake()
            {
                var serviceResponse = AuthService.GenerateJwtToken();
                    return Ok(serviceResponse);
            }
        }
    }
}

