using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces._bases;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IInstallmentRepository : IRepositoryBase<Installment>
    {
        Task<IEnumerable<Installment>> GetAllByContractId(Guid contractUuid);
    }
}
