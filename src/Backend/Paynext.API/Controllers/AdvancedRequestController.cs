using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Interfaces;
using Paynext.Domain.Errors;
using System.Security.Claims;

namespace Paynext.API.Controllers
{
    [ApiController]
    [Route("api/advanced-request")]
    [ApiVersion("1.0")]
    public class AdvancedRequestController(IPayManagement payManagement) : ControllerBase
    {
        private readonly IPayManagement _payManagement = payManagement;

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    // Check if the user is authenticated
        //    if (!HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return Unauthorized();
        //    }
        //    // Check if the user has the required role
        //    if (!HttpContext.User.IsInRole("Admin"))
        //    {
        //        return Forbid();
        //    }
        //    // Extract user ID from claims
        //    var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        //    {
        //        return Unauthorized(Error.UNAUTHORIZED);
        //    }
        //    return Ok();
        //}
        [HttpPost, Authorize]
        public async Task<IActionResult> RequestAntecipation([FromBody] Guid request)
        {
            // Check if the user is authenticated
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            // Check if the user has the required role
            if (HttpContext.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            // Extract user ID from claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }
            // Process the request using the business logic layer
            var response = await _payManagement.AntecipationInstallmentRequest(request, userId);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetAllAntecipationRequest(int pagerNumber, int pageSize)
        {
            // Check if the user is authenticated
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            // Extract user ID from claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }
            if (HttpContext.User.IsInRole("Client"))
            {
                var responseClient = await _payManagement.ListUserAntecipationRequests(userId, pagerNumber, pageSize);
                return StatusCode((int)responseClient.StatusCode, responseClient.ApiReponse);
            }

            var response = await _payManagement.ListAllAntecipationRequests(pagerNumber, pageSize);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }
        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetAntecipationRequestById(Guid guid)
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
            // Process the request using the business logic layer
            var response = await _payManagement.GetInstallment(guid);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }
        [HttpPut("approve"), Authorize]
        public async Task<IActionResult> ApproveRequest([FromBody] List<ActioneInstallment> installments)
        {

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
            // Process the request using the business logic layer
            var response = await _payManagement.ActioneAntecipationRequests(installments, userId);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }
    }
}
