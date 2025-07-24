using Paynext.Application.Dtos.Entities;
using Paynext.Application.Dtos.Entities.Installment;
using SharedKernel;

namespace Paynext.Application.Interfaces
{
    public interface IPayManagement
    {
        Task<Response<bool>> AntecipationInstallmentRequest(Guid installmentUuid, Guid userUuid);
        Task<Response<List<ContractInformationDto>>> ListAllAntecipationRequests(int pageNumber, int pageSize);
        Task<Response<List<ContractInformationDto>>> ListUserAntecipationRequests(Guid guid, int pageNumber, int pageSize);

        Task ActioneAntecipationRequests(List<ActioneInstallment> installments, Guid userUuid);
        Task<Response<ContractInformationDto>> GetInstallment(Guid guid);
    }
}
