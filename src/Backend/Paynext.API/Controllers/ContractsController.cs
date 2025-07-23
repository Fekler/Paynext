using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Interfaces;

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

    }
}
