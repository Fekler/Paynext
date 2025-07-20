using Microsoft.Extensions.Logging;
using Paynext.Application.Authentication;
using Paynext.Application.Interfaces;
using Paynext.Application.Interfaces.UseCases;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Domain.Errors;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Application.UseCases
{
    public class UserAuthentication(IUserBusiness userBusiness, ITokenService tokenService, ILogger<UserAuthentication> logger) : IUserAuthentication
    {
        private readonly IUserBusiness _userBusiness = userBusiness;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ILogger<UserAuthentication> _logger = logger;

        public async Task<Response<AuthenticationResponse>> Login(string email, string password)
        {
            var response = new Response<AuthenticationResponse>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ApiReponse = new ApiResponse<AuthenticationResponse>
                {
                    Success = false,
                    Data = null,
                    ErrorCode = (int)HttpStatusCode.Unauthorized,
                    Message = Error.INVALID_EMAIL_OR_PASSWORD,
                }
            };

            try
            {
                var userResult = await _userBusiness.Get(email);
                if (userResult.ApiReponse.Data is null)
                {
                    return response;
                }

                if (!BCryptService.VerifyPassword(password, userResult.ApiReponse.Data.Password))
                {
                    return response;
                }

                var authenticationResult = await AuthenticateUser(userResult.ApiReponse.Data);
                if (!authenticationResult.Success || authenticationResult.Data is null)
                {
                    response.ApiReponse = authenticationResult;
                    return response;
                }

                response.StatusCode = HttpStatusCode.OK;
                response.ApiReponse.ErrorCode = null;

                response.ApiReponse = authenticationResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ApiReponse.Success = false;
                response.ApiReponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                response.ApiReponse.Message = Error.UNEXPECTED_ERROR + " during login.";
            }

            return response;

        }

        private async Task<ApiResponse<AuthenticationResponse>> AuthenticateUser(User user)
        {
            ApiResponse<AuthenticationResponse> response = new()
            {
                Success = true,
                Data = new AuthenticationResponse(),
                ErrorCode = 0,
                Message = Const.MESSAGE_LOGIN_SUCCESS,
            };

            var userToken = new UserAuthenticateJWT
            {
                UserUuid = user.UUID,
                UserEmail = user.Email,
                UserName = user.FullName,
                UserRole = user.UserRole.ToString(),
            };

            var accessTokenResult = await _tokenService.GenerateJwtToken(userToken);
            response.Data.AccessToken = accessTokenResult.AccessToken;
            response.Data.ExpiresIn = accessTokenResult.ExpiresIn;

            return response;
        }
    }
}