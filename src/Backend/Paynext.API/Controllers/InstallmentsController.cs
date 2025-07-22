using Microsoft.AspNetCore.Mvc;
using Paynext.Application.Interfaces;

namespace Paynext.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InstallmentsController (IInstallmentBusiness installmentBusiness): ControllerBase
    {
        private readonly IInstallmentBusiness _installmentBusiness = installmentBusiness;

        
    }
}
