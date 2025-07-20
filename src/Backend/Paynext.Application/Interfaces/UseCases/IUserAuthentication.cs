using Paynext.Application.Authentication;
using SharedKernel;

namespace Paynext.Application.Interfaces.UseCases
{
    public interface IUserAuthentication
    {
        Task<Response<AuthenticationResponse>> Login(string email, string password);
    }
}
