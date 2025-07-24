using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Dtos.Entities.User;
using Paynext.Application.Interfaces;
using Paynext.Domain.Errors;
using System;
using System.Security.Claims;

namespace Paynext.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class UsersController(IUserBusiness userBusiness) : ControllerBase
    {
        private readonly IUserBusiness _userBusiness = userBusiness;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userBusiness.Add(createUserDto);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        //[HttpDelete("{id:int}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _userBusiness.Delete(id);
        //    return StatusCode((int)response.StatusCode, response.ApiReponse);
        //}

        [HttpDelete("{guid:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid guid)
        {

            if (!HttpContext.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }
            if( userId == guid)
            {
                return BadRequest("You cannot delete your own account.");
            }
            var response = await _userBusiness.Delete(guid);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _userBusiness.Get(id);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpGet("{guid:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var response = await _userBusiness.GetDto(guid);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userBusiness.GetAll();
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!HttpContext.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }
            if (userId == updateUserDto.UUID)
            {
                return BadRequest("You cannot Update your own account.");
            }
            var response = await _userBusiness.Update(updateUserDto);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [AllowAnonymous]
        [HttpPut("change-password")]
        [Authorize()]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userBusiness.ChangePasswordAsync(changePasswordDto);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

    }
}
