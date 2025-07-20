using Microsoft.EntityFrameworkCore;
using Paynext.Domain.Entities._bases;
using Paynext.Domain.Interfaces._bases;
using Paynext.Infra.Context;


namespace Paynext.Infra.Repositories._bases
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<Guid> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.UUID;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid guid)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.UUID == guid);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(int id) => await _dbSet.FindAsync(id);

        public async Task<T> Get(Guid uuid) => await _dbSet.FirstOrDefaultAsync(e => e.UUID == uuid);

        public async Task<bool> Update(T entity)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.UUID == entity.UUID);
            if (existingEntity == null)
                return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Guid>> AddRangeOfEntitiesAsync(List<T> entityList)
        {
            if (entityList != null && entityList.Count != 0)
            {
                await _context.Set<T>().AddRangeAsync(entityList);
                await _context.SaveChangesAsync();
                return [.. entityList.Select(b => b.UUID)];

            }
            return [];
        }
    }
}
