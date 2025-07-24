using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Interfaces._bases;
using Paynext.Domain.Entities;
using SharedKernel;

namespace Paynext.Application.Interfaces
{
    public interface IContractBusiness : IBusinessBase<Contract, CreateContractDto, UpdateContractDto, ContractDto>
    {
        Task<Response<List<ContractDto>>> GetAllByUserFullInformation(Guid userUuid);
        Task<Response<List<ContractDto>>> GetByUser(Guid userUuid);

        Task<Response<ContractDto>> GetFullInformation(Guid contractUuid, Guid userUuid, bool admin = false);
        Task<Response<ContractDto>> GetFullInformationByContractNumber(string contracNumber, Guid userUuid, bool admin = false);


        Task<Response<List<ContractDto>>> GetAllFullInformation();

        Task<Response<List<ContractDto>>> GetAllDto();
    }
}
