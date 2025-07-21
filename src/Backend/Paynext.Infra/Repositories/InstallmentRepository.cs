using Microsoft.EntityFrameworkCore;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Infra.Context;
using Paynext.Infra.Repositories._bases;

namespace Paynext.Infra.Repositories
{
    public class InstallmentRepository(AppDbContext context) : RepositoryBase<Installment>(context), IInstallmentRepository
    {
        public async Task<IEnumerable<Installment>> GetAllByContractId(Guid contractUuid)
        {
            return await _dbSet.Where(i => i.ContractUuid == contractUuid)
                .ToListAsync();
        }
    }
}
