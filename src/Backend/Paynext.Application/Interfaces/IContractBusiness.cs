using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Interfaces._bases;
using Paynext.Domain.Entities;
using SharedKernel;

namespace Paynext.Application.Interfaces
{
    public interface IContractBusiness : IBusinessBase<Contract, CreateContractDto, UpdateContractDto, ContractDto>
    {
        Task<Response<List<ContractDto>>> GetByUser(Guid userUuid);
        Task<Response<ContractDto>> GetFullInformation(Guid guid);

        Task<Response<List<ContractDto>>> GetAllFullInformation();
    }
}
