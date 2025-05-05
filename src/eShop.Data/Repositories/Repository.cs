using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return false;
            _entities.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
