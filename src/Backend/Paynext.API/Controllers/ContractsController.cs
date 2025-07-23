using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Business;
using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Dtos.Entities.User;
using Paynext.Application.Interfaces;
using Paynext.Domain.Errors;
using System.Security.Claims;

namespace Paynext.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContractsController(IContractBusiness contractBusiness) : ControllerBase
    {
        private readonly IContractBusiness _contractBusiness = contractBusiness;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateContractDto createContractDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _contractBusiness.Add(createContractDto);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }
        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var response = await _contractBusiness.Get(id);
        //    return StatusCode((int)response.StatusCode, response.ApiReponse);
        //}
        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var response = await _contractBusiness.GetFullInformation(guid);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }


        [HttpDelete("{guid:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var response = await _contractBusiness.Delete(guid);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpGet()]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(Error.UNAUTHORIZED);
            }if (HttpContext.User.IsInRole("Client"))
            {
                var responseClient = await _contractBusiness.GetByUser(userId);
                return StatusCode((int)responseClient.StatusCode, responseClient.ApiReponse);
            }
            var response = await _contractBusiness.GetAllFullInformation();
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateContractDto updateContractDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _contractBusiness.Update(updateContractDto);
            return StatusCode((int)response.StatusCode, response.ApiReponse);
        }

    }
}
