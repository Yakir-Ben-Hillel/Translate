using Translation.API.Models.Responses;

namespace Translation.API.Services
{
    public interface IAuthService
    {
        public ServiceResponse<TokenResponse> GenerateJwtToken();
    }
}
