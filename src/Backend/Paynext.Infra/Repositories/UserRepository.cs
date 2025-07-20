using Microsoft.EntityFrameworkCore;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Infra.Context;
using Paynext.Infra.Repositories._bases;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Infra.Repositories
{
    public class UserRepository(AppDbContext context) : RepositoryBase<User>(context), IUserRepository
    {
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllByRole(UserRole role)
        {
            return await _dbSet
                .Where(u => u.UserRole == role)
                .ToListAsync();
        }

        public async Task<User> GetByDocument(string document)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Document == document);
        }

        public async Task<User> GetByEmail(string email)

        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByPhone(string phone)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Phone == phone);
        }
    }
}
