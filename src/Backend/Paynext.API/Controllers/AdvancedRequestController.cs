using Microsoft.AspNetCore.Mvc;
using Paynext.Domain.Errors;
using System.Security.Claims;

namespace Paynext.API.Controllers
{
    [ApiController]
    [Route("api/advanced-request")]
    [ApiVersion("1.0")]
    public class AdvancedRequestController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            // Check if the user is authenticated
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            // Check if the user has the required role
            if (!HttpContext.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            // Extract user ID from claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }
            return Ok();
        }
    }
}
