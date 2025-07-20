using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Interfaces.UseCases;

namespace Paynext.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [ApiVersion("1.0")]
    public class AuthController(IUserAuthentication authenticationUseCase) : ControllerBase
    {
        private readonly IUserAuthentication _authenticationUseCase = authenticationUseCase;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authenticationUseCase.Login(request.Email, request.Password);
                return StatusCode((int)response.StatusCode, response.ApiReponse);

            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Login failed: {ex.Message}" });
            }
        }

    }
}