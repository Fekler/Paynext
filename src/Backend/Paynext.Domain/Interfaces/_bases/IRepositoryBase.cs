using Paynext.Domain.Entities._bases;

namespace Paynext.Domain.Interfaces._bases
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<Guid> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(Guid uuid);
        Task<T> Get(int id);
        Task<T> Get(Guid uuid);
        Task<bool> Update(T entity);
    }
}