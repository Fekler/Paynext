using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces._bases;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByDocument(string document);
        Task<User> GetByPhone(string phone);
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> GetAllByRole(UserRole role);

    }
}
