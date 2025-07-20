using Paynext.Domain.Entities;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByDocument(string document);
        Task<User> GetByPhone(string phone);
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> GetAllByRole(UserRole role);

    }
}
