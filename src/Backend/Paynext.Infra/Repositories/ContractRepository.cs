using Microsoft.EntityFrameworkCore;
using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Infra.Context;
using Paynext.Infra.Repositories._bases;
using System;

namespace Paynext.Infra.Repositories
{
    public class ContractRepository(AppDbContext context) : RepositoryBase<Contract>(context), IContractRepository
    {
        public async Task<IEnumerable<Contract>> GetAllActiveContracts()
        {
            return await _dbSet
                .Where(c => c.IsActive && !c.IsFinished)
                .ToListAsync();
        }
        public async Task<Contract> GetByContractNumber(string contractNumber)
        {
            return await _dbSet
                .Where(c => c.ContractNumber == contractNumber)
                .Include(c => c.Installments.OrderBy(i => i.DueDate))
                    .ThenInclude(i => i.ActionedByUser)
                .Include(c => c.User)
                .FirstOrDefaultAsync();
        }
        public async Task<Contract> GetFullInformationByUuid(Guid uuid)
        {
            return await _dbSet
                .Where(c => c.UUID == uuid)
                .Include(c => c.Installments.OrderBy(i => i.DueDate))
                    .ThenInclude(i => i.ActionedByUser)
                .Include(c => c.User)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Contract>> GetAllFullInformation()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .Include(c => c.Installments.OrderBy(i => i.DueDate))
                    .ThenInclude(i => i.ActionedByUser)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contract>> GetByUserUuid(Guid userUuid)
        {
            return await _dbSet
                .Where(c => c.UserUuid == userUuid)
                .ToListAsync();
        }
        public async Task<List<Contract>> ContractDtoByUserUuid(Guid uuid)
        {

            var contracts = await _dbSet
                .Where(c => c.UserUuid == uuid)
                .Include(c => c.Installments.OrderBy(i => i.DueDate))
                    .ThenInclude(i => i.ActionedByUser)
                .Include(c => c.User)
                .ToListAsync();

            return contracts;

        }
    }
}
