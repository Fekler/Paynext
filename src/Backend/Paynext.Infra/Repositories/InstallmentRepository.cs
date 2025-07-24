using Microsoft.EntityFrameworkCore;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Infra.Context;
using Paynext.Infra.Repositories._bases;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Infra.Repositories
{
    public class InstallmentRepository(AppDbContext context) : RepositoryBase<Installment>(context), IInstallmentRepository
    {
        public async Task<IEnumerable<Installment>> GetAllByContractId(Guid contractUuid)
        {
            return await _dbSet.Where(i => i.ContractUuid == contractUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetAllByContractIdAndStatus(Guid contractUuid, InstallmentStatus status)
        {
            return await _dbSet.Where(i => i.ContractUuid == contractUuid && i.Status == status)
                .ToListAsync();
        }
        public async Task<List<Installment>> GetAllAntecipateToActione(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Where(i => i.IsAntecipated && i.Status == InstallmentStatus.Open && i.ActionedByUser == null)
                .Include(i => i.Contract)
                    .ThenInclude(i => i.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
