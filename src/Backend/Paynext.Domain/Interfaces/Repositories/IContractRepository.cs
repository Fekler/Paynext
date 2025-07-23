using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces._bases;
using System.Threading.Tasks;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IContractRepository :IRepositoryBase<Contract>
    {
        Task<IEnumerable<Contract>> GetAllActiveContracts();
        Task<Contract> GetByContractNumber(string contractNumber);
        Task<Contract> GetFullInformationByUuid(Guid uuid);

        Task<IEnumerable<Contract>> GetByUserUuid(Guid userUuid);
        Task<List<Contract>> GetAllFullInformation();
    }
}
