using Paynext.Application.Authentication;

namespace Paynext.Application.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticationResponse> GenerateJwtToken(UserAuthenticateJWT userAuthenticateJwt);
        int GetTokenLifetimeInMinutes();
    }
}
