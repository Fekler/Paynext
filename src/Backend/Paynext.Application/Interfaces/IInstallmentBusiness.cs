using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Interfaces._bases;
using Paynext.Domain.Entities;
using SharedKernel;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Interfaces
{
    public interface IInstallmentBusiness : IBusinessBase<Installment, CreateInstallmentDto, UpdateInstallmentDto, InstallmentDto>
    {
        Task<Response<List<InstallmentDto>>> GetByContract(Guid contractUuid);
        Task<Response<List<InstallmentDto>>> GetByContractAndStatus(Guid contractUuid, InstallmentStatus status);
    }
}
