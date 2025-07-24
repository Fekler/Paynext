using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Domain.Interfaces._bases;
using System.Threading.Tasks;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IInstallmentRepository : IRepositoryBase<Installment>
    {
        Task<IEnumerable<Installment>> GetAllByContractId(Guid contractUuid);
        Task<IEnumerable<Installment>> GetAllByContractIdAndStatus(Guid contractUuid, Enums.InstallmentStatus status);
        Task<List<Installment>> GetAllAntecipateToActione(int pageNumber, int pageSize);
        Task<List<Installment>> GetUserAntecipateToActione(Guid userUuid, int pageNumber, int pageSize);
    }
}
