using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces._bases;
using System.Threading.Tasks;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IContractRepository :IRepositoryBase<Contract>
    {
        Task<IEnumerable<Contract>> GetAllActiveContracts();
        Task<Contract> GetByContractNumber(string contractNumber);
        Task<IEnumerable<Contract>> GetByUserUuid(Guid userUuid);
        Task<Contract> GetFullInformationByUuid(Guid uuid);
    }
}
